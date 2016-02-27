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
using WebApp.ViewModels.InvoiceViewModels;
using Model.Enums;
using System.Globalization;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();
        // GET: Invoice
        public async Task<ActionResult> Index(FilterArgs args)
        {
            var thisMonth = new DateTime(SystemTime.Now().Year, SystemTime.Now().Month, 1);
            var nextMonth = thisMonth.AddMonths(1);
            if (args.Year.HasValue && args.Month.HasValue)
            {
                thisMonth = new DateTime(args.Year.Value, args.Month.Value, 1);
                nextMonth = thisMonth.AddMonths(1);
            }
            args.Year = thisMonth.Year;
            args.Month = thisMonth.Month;
            args.FilterDate = thisMonth;

            var vm = new IndexViewModel();

            vm.FilterArgs = args;

            vm.CurrentUser = db.Users.Single(u => u.UserName == User.Identity.Name);

            //vm.SelectedServiceProvider = await db.BillableEntities.SingleOrDefaultAsync(u => u.EntityGuid == args.ServiceProviderId);
            //vm.SelectedCustomer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == args.CustomerId);
            IQueryable<Invoice> query = db.Invoices.Where(c => c.InvoiceDate >= thisMonth && c.InvoiceDate < nextMonth);
            if (args.ServiceProviderId.HasValue && args.CustomerId.HasValue)
            {
                query = query.Where(i => i.ServiceProviderGuid == args.ServiceProviderId && i.CustomerGuid == args.CustomerId);
            }
            else if (args.ServiceProviderId.HasValue && !args.CustomerId.HasValue)
            {
                query = query.Where(i => i.ServiceProviderGuid == args.ServiceProviderId);
            }
            else if (!args.ServiceProviderId.HasValue && args.CustomerId.HasValue)
            {
                query = query.Where(i => i.CustomerGuid == args.CustomerId);
            }
            vm.Invoices = await query.ToListAsync();
            
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Create(short ServiceRequestId)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(ServiceRequestId);

            var serviceProvider = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
            var customer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

            // Automap to an invoice object
            var service = new InvoiceService();
            serviceRequest.ServiceRequestPrice = serviceRequest.EffectivePrice;
            var invoiceNumber = db.GetNextInvoiceNumber().SingleOrDefault();
            var invoice = service.PreviewInvoice(invoiceNumber, serviceProvider, customer, serviceRequest);

            using (var context = new OrvosiEntities(User.Identity.Name))
            {
                context.Invoices.Add(invoice);
                await context.SaveChangesAsync();
            }

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return RedirectToAction("Details", new { id = invoice.Id });
        }

        public async Task<ActionResult> Details(int id)
        {
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.ReadOnly;
            return View(obj);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.Edit;
            return View("Details", obj);
        }

        [HttpPost]
        public async Task<ActionResult> AddInvoiceDetail(InvoiceDetail InvoiceDetail)
        {
            db.InvoiceDetails.Add(InvoiceDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = InvoiceDetail.InvoiceId });
        }

        [HttpPost]
        public async Task<ActionResult> EditInvoiceDetail(int id)
        {
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.Edit;
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteInvoiceDetail(int id)
        {
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.Edit;
            return View(obj);
        }
    }
}