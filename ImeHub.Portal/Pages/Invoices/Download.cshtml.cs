using ImeHub.Portal.Data;
using ImeHub.Portal.Data.Invoices;
using ImeHub.Portal.Library;
using ImeHub.Portal.Library.Security;
using ImeHub.Portal.Services.DateTimeService;
using ImeHub.Portal.Services.FileSystem;
using ImeHub.Portal.Services.HtmlToPdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

namespace ImeHub.Portal.Pages.Invoices
{
    public class DownloadModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly FileSystemFactory _fileSystemFactory;
        private readonly IDateTime _dateTime;
        private readonly IRazorToStringViewRenderer _razor;
        private readonly IHtmlToPdf _htmlToPdf;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public DownloadModel(ApplicationDbContext dbContext, IConfiguration configuration, FileSystemFactory fileSystemFactory, IDateTime dateTime, IRazorToStringViewRenderer razor, IHtmlToPdf htmlToPdf, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _fileSystemFactory = fileSystemFactory;
            _dateTime = dateTime;
            _razor = razor;
            _htmlToPdf = htmlToPdf;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var link = await _dbContext.InvoiceDownloadLinks
                .Include(idl => idl.Invoice)
                .SingleOrDefaultAsync(c => c.ObjectGuid == id);

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
