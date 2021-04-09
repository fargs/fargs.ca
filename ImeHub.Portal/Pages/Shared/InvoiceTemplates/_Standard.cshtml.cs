using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImeHub.Portal.Data.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImeHub.Portal.Pages.Shared.InvoiceTemplates
{
    public class StandardModel : PageModel
    {
        public Invoice Invoice { get; set; }
    }
}
