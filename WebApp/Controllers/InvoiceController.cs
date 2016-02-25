using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Model.Extensions;
using WebApp.Library.Enums;
using WebApp.Library;
using WebApp.Services;
using System.Data.Entity;

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

        public async Task<ActionResult> Create(short id)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(id);

            var serviceProvider = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
            var customer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

            // Automap to an invoice object
            var service = new InvoiceService();
            serviceRequest.ServiceRequestPrice = serviceRequest.EffectivePrice;
            var invoice = service.PreviewInvoice(serviceProvider, customer, serviceRequest);

            // Confirm the examination was completed.
            var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);
            ViewBag.IsValidToSubmitInvoice = intakeInterviewTask.CompletedDate.HasValue ? true : false;
            ViewBag.FormMode = FormModes.Add;
            return View("Details", invoice);
        }

        [HttpPost]
        public async Task<ActionResult> Create(decimal? Amount, string AdditionalNotes, int ServiceRequestId)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(ServiceRequestId);

            var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == ServiceRequestId && c.TaskId == Model.Enums.Tasks.IntakeInterview);
            if (!intakeInterviewTask.CompletedDate.HasValue)
            {
                return RedirectToAction("Create", new { id = serviceRequest.Id });
            }
            if (ModelState.IsValid)
            {
                var serviceProvider = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
                var customer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

                // Automap to an invoice object
                var service = new InvoiceService();
                serviceRequest.ServiceRequestPrice = Amount;
                var invoice = service.PreviewInvoice(serviceProvider, customer, serviceRequest);
                //TODO: Clean up the below line
                invoice.InvoiceDetails.First().AdditionalNotes = AdditionalNotes;
                // Confirm the examination was completed.
                using (var context = new OrvosiEntities(User.Identity.Name))
                {
                    context.Invoices.Add(invoice);
                    await context.SaveChangesAsync();
                }
                return RedirectToAction("Details", new { id = invoice.Id });
            }
            return RedirectToAction("Create", new { id = serviceRequest.Id });
        }

        public async Task<ActionResult> Details(int id)
        {
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.ReadOnly;
            return View(obj);
        }
    }
}