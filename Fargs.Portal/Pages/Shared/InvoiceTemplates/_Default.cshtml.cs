using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fargs.Portal.Data.Invoices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fargs.Portal.Pages.Shared.InvoiceTemplates
{
    public class DefaultModel : PageModel
    {
        public Invoice Invoice { get; set; }
    }
}