﻿using Fargs.Portal.Data;
using Fargs.Portal.Data.Invoices;
using Fargs.Portal.Library;
using Fargs.Portal.Library.Security;
using Fargs.Portal.Services.DateTimeService;
using Fargs.Portal.Services.FileSystem;
using Fargs.Portal.Services.HtmlToPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fargs.Portal.Pages.Invoices
{
    public class DownloadModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FileSystemFactory _fileSystemFactory;
        private readonly IDateTime _dateTime;
        private readonly IRazorToStringViewRenderer _razor;
        private readonly IHtmlToPdf _htmlToPdf;
        private readonly ClaimsPrincipal _user;

        public DownloadModel(ApplicationDbContext dbContext, IConfiguration configuration, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, FileSystemFactory fileSystemFactory, IDateTime dateTime, IRazorToStringViewRenderer razor, IHtmlToPdf htmlToPdf, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _fileSystemFactory = fileSystemFactory;
            _dateTime = dateTime;
            _razor = razor;
            _htmlToPdf = htmlToPdf;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var userId = _user.UserId();

            var companyIdsAccessList = await _dbContext.CompanyAccesses
                .Where(c => c.UserId == userId)
                .Select(c => c.CompanyRole.Company.ObjectGuid)
                .ToArrayAsync();

            var link = await _dbContext.InvoiceDownloadLinks
                .Include(idl => idl.Invoice)
                .Where(idl => companyIdsAccessList.Contains(idl.Invoice.CustomerGuid))
                .Where(idl => idl.ObjectGuid == id)
                .SingleOrDefaultAsync();

            // Check if the link exists and they have access (we don't tell them it exists)
            if (link == null)
            {
                return NotFound();
            }

            // Check if the link has expired
                if (_dateTime.Now > link.ExpiryDate)
            {
                return RedirectToActionPermanent("Invoices/DownloadLinkExpired");
            }

            if (link.Invoice == null)
            {
                return NotFound();
            }

            var fileSystem = _fileSystemFactory.Create(Enum.Parse<FileSystemProvider>(link.Invoice.FileSystemProvider), _configuration);
            var file = await fileSystem.DownloadFileAsync(link.Invoice.FileId);

            return File(file, "application/octet-stream", $"Invoice_{link.Invoice.InvoiceNumber}_{link.Invoice.ObjectGuid}.pdf");
        }
    }

}