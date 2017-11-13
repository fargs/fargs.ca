using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Reports.Data;
using WebApp.Controllers;
using WebApp.Library;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Reports.Controllers
{
    public class InvoiceController : BaseController
    {
        private ReportsContext context;

        public InvoiceController(ReportsContext context)
        {
            this.context = context;
        }
        // GET: Reports/Invoice
        public ActionResult ForReconciliation()
        {
            var model = context.InvoiceToQbExports.ToArray();
            var result = new CsvResult(model, false, true)
            {
                FileName = "orvosi_invoices"
            };
            return result;
        }
    }
}