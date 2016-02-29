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
using System.Text;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        public const byte PaymentDueInDays = 14;
        public const decimal TaxRateHst = 0.13M;

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

            var invoiceNumber = db.GetNextInvoiceNumber().SingleOrDefault();

            var invoice = new Invoice()
            {
                InvoiceNumber = invoiceNumber,
                InvoiceDate = serviceRequest.AppointmentDate.Value,
                DueDate = SystemTime.Now().AddDays(PaymentDueInDays),
                Currency = "CAD",
                ServiceProviderGuid = serviceProvider.EntityGuid,
                ServiceProviderName = serviceProvider.EntityName,
                ServiceProviderEntityType = serviceProvider.EntityType,
                ServiceProviderLogoCssClass = serviceProvider.LogoCssClass,
                ServiceProviderAddress1 = serviceProvider.Address1,
                ServiceProviderAddress2 = serviceProvider.Address2,
                ServiceProviderCity = serviceProvider.PostalCode,
                ServiceProviderPostalCode = serviceProvider.PostalCode,
                ServiceProviderProvince = serviceProvider.ProvinceName,
                ServiceProviderCountry = serviceProvider.CountryName,
                ServiceProviderEmail = serviceProvider.BillingEmail,
                ServiceProviderPhoneNumber = serviceProvider.Phone,
                CustomerGuid = customer.EntityGuid,
                CustomerName = customer.EntityName,
                CustomerEntityType = customer.EntityType,
                CustomerAddress1 = customer.Address1,
                CustomerAddress2 = customer.Address2,
                CustomerCity = customer.PostalCode,
                CustomerPostalCode = customer.PostalCode,
                CustomerProvince = customer.ProvinceName,
                CustomerCountry = customer.CountryName,
                CustomerEmail = customer.BillingEmail,
                TaxRateHst = TaxRateHst
            };

            var invoiceDetail = new InvoiceDetail()
            {
                ServiceRequestId = serviceRequest.Id,
                Rate = GetInvoiceDetailRate(serviceRequest.IsNoShow, serviceRequest.NoShowRate, serviceRequest.IsLateCancellation, serviceRequest.LateCancellationRate)
            };

            var description = new StringBuilder();
            description.AppendLine(serviceRequest.ClaimantName);
            description.AppendLine(serviceRequest.ServiceName);
            description.AppendLine(serviceRequest.City);
            if (serviceRequest.IsNoShow)
            {
                description.AppendLine(string.Format("NO SHOW - RATE {0}", invoiceDetail.Rate.Value.ToString("0%")));
            }
            else if (serviceRequest.IsLateCancellation)
            {
                description.AppendLine(string.Format("LATE CANCELLATION - RATE {0}", invoiceDetail.Rate.Value.ToString("0%")));
            }

            invoiceDetail.Description = description.ToString();
            invoiceDetail.Amount = GetInvoiceDetailAmount(serviceRequest.EffectivePrice, invoiceDetail.Rate);
            invoiceDetail.ModifiedUser = User.Identity.Name;
            invoiceDetail.ModifiedDate = SystemTime.Now();

            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.SubTotal = invoiceDetail.Amount;
            invoice.Total = GetInvoiceTotal(invoice.SubTotal, TaxRateHst);
            invoice.ModifiedUser = User.Identity.Name;
            invoice.ModifiedDate = SystemTime.Now();

            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return PartialView("_InvoiceTable", invoice.InvoiceDetails.ToList());
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
        public async Task<ActionResult> Edit(InvoiceDetail updatedInvoiceDetail)
        {
            var id = int.Parse(Request.Form.Get("Id"));

            var invoiceDetail = await db.InvoiceDetails.FindAsync(id);
            var invoice = invoiceDetail.Invoice;

            //var updatedInvoiceDetail = updatedInvoice.InvoiceDetails.First();
            var serviceRequest = await db.ServiceRequests.FindAsync(invoiceDetail.ServiceRequestId);
            invoiceDetail.AdditionalNotes = updatedInvoiceDetail.AdditionalNotes;
            invoiceDetail.Rate = GetInvoiceDetailRate(serviceRequest.IsNoShow, serviceRequest.NoShowRate, serviceRequest.IsLateCancellation, serviceRequest.LateCancellationRate);
            invoiceDetail.Amount = GetInvoiceDetailAmount(updatedInvoiceDetail.Amount, invoiceDetail.Rate);

            invoice.SubTotal = updatedInvoiceDetail.Amount;
            invoice.Total = GetInvoiceTotal(invoice.SubTotal, invoice.TaxRateHst);

            invoice.ModifiedUser = User.Identity.Name;
            invoiceDetail.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return RedirectToAction("Details", new { id = invoice.Id });
        }

        public async Task<ActionResult> Submit(int id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.Id == id);

            var messageService = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), HttpContext.Request.Url.GetLeftPart(UriPartial.Authority));
            await messageService.SendInvoice(invoice.CustomerEmail, invoice.InvoiceDetails.First());

            invoice.SentDate = SystemTime.Now();
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();
            return PartialView("_InvoiceSubmitted", invoice.SentDate.Value);
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

        private decimal? GetInvoiceDetailRate(bool isNoShow, decimal? noShowRate, bool isLateCancellation, decimal? lateCancellationRate)
        {
            if (isNoShow)
            {
                if (!noShowRate.HasValue)
                {
                    throw new Exception("No show rate must have a value");
                }
                return noShowRate;
            }
            else if (isLateCancellation)
            {
                if (!lateCancellationRate.HasValue)
                {
                    throw new Exception("Late cancellation rate must have a value");
                }
                return lateCancellationRate;
            }
            else
            {
                return 1;
            }
        }

        private decimal? GetInvoiceDetailAmount(decimal? price, decimal? rate)
        {
            return price * rate;
        }

        public decimal? GetInvoiceTotal(decimal? subTotal, decimal? taxRate)
        {
            return subTotal * (1 + taxRate);
        }
    }
}