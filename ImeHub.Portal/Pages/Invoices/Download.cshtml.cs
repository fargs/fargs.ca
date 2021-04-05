using ImeHub.Portal.Data;
using ImeHub.Portal.Services.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Pages.Invoices
{
    public class DownloadModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileSystemProvider _fileSystem;

        public DownloadModel(ApplicationDbContext dbContext, IFileSystemProvider fileSystem)
        {
            _dbContext = dbContext;
            _fileSystem = fileSystem;
        }

        public IActionResult OnGet(Guid id)
        {
            //var link = await _dbContext.InvoiceDownloadLinks
            //    .Include(dl => dl.Invoice)
            //    .Include(dl => dl.InvoiceDownloads)
            //    .SingleOrDefaultAsync(c => c.ObjectGuid == form.DownloadLinkGuid);

            var filePath = Path.Combine(_fileSystem.RootPath, "TestInvoice.pdf");

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", $"{id}.pdf");
        }
    }

}
