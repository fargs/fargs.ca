using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImeHub.Portal.Data;
using ImeHub.Portal.Pages.Shared.InvoiceTemplates;
using ImeHub.Portal.Services.DateTimeService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ImeHub.Portal.Pages.Invoices
{
    public class PreviewModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;

        public StandardModel StandardModel { get; set; }

        public PreviewModel(ApplicationDbContext dbContext, IDateTime dateTime)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
        }
        public async Task<ActionResult> OnGetAsync(Guid id)
        {
            var link = await _dbContext.InvoiceDownloadLinks
                .SingleOrDefaultAsync(c => c.ObjectGuid == id);

            // Check if the link exists and they have access (we don't tell them it exists)
            if (link == null)
            {
                return NotFound();
            }

            // Check if the link has expired
            if (_dateTime.Now > link.ExpiryDate)
            {
                return NotFound();
            }

            var invoice = _dbContext.Invoices
                .Include(i => i.InvoiceDetails)
                .SingleOrDefault(i => i.Id == link.InvoiceId);

            if (invoice == null)
            {
                return NotFound();
            }

            var standardModel = new StandardModel();
            standardModel.Invoice = invoice;

            StandardModel = standardModel;

            return Page();
        }
    }
}
