using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Preview(int id)
        {
            var obj = await db.PhysicianToCompanyServiceRequestInvoicePreviews.FindAsync(id);
            return View(obj);
        }
    }
}