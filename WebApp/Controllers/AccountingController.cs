using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.AccountingModel;
using WebApp.Library.Extensions;
using System.Threading.Tasks;
using System.Data.Entity;
using Orvosi.Shared.Enums;
using System.Net;
using System.Globalization;
using WebApp.ViewModels.InvoiceViewModels;
using WebApp.Library;
using System.Net.Mail;
using System.IO;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Case Coordinator, Super Admin, Physician")]
    public class AccountingController : Controller
    {
        // GET: Accounting
        public ActionResult Invoices(Guid? serviceProviderId)
        {
            Guid userId = GetServiceProviderId(serviceProviderId);

            var model = new Mapper(new Orvosi.Data.OrvosiDbContext()).MapToServiceRequests(userId, SystemTime.Now());

            ViewBag.SelectedUserId = userId;

            return View(model);
        }

        // API
        public async Task<ActionResult> SendInvoice(int invoiceId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var invoice = await context.Invoices.FirstAsync(c => c.Id == invoiceId);
            
            var message = BuildSendInvoiceMailMessage(invoice, Request.GetBaseUrl());

            // this should get created using a DI container and configured in the Startup.
            await new GoogleServices()
                .SendEmailAsync(message);

            invoice.SentDate = SystemTime.Now();
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;

            invoice.InvoiceSentLogs.Add(new Orvosi.Data.InvoiceSentLog()
            {
                InvoiceId = invoice.Id,
                EmailTo = invoice.CustomerEmail,
                SentDate = SystemTime.Now(),
                ModifiedDate = SystemTime.Now(),
                ModifiedUser = User.Identity.Name
            });

            foreach (var item in invoice.InvoiceDetails)
            {
                var task = item.ServiceRequest.ServiceRequestTasks.FirstOrDefault(c => c.TaskId == Tasks.SubmitInvoice); // TODO: 37 is submit invoice for add/pr
                task.CompletedDate = SystemTime.Now();
            }

            await context.SaveChangesAsync();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private MailMessage BuildSendInvoiceMailMessage(Orvosi.Data.Invoice invoice, string baseUrl)
        {
            var message = new MailMessage();
            message.To.Add(invoice.CustomerEmail);
            message.From = new MailAddress(invoice.ServiceProviderEmail);
            message.Subject = string.Format("Invoice {0} - {1} - Payment Due {2}", invoice.InvoiceNumber, invoice.ServiceProviderName, invoice.DueDate.Value.ToOrvosiDateFormat());
            message.IsBodyHtml = true;
            message.Bcc.Add("lfarago@orvosi.ca,afarago@orvosi.ca");

            var templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views/Shared/NotificationTemplates/Invoice.html");

            ViewData["BaseUrl"] = baseUrl; //This is needed because the full address needs to be included in the email download link
            message.Body = WebApp.Library.Helpers.HtmlHelpers.RenderViewToString(this.ControllerContext, "~/Views/Shared/NotificationTemplates/Invoice.cshtml", invoice);

            return message;
        }

        [HttpPost]
        public ActionResult Create(int serviceRequestId)
        {
            var repo = new Mapper(new Orvosi.Data.OrvosiDbContext());
            repo.Create(serviceRequestId, User);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<ActionResult> Update(EditInvoiceDetailForm form)
        {
            var context = new Orvosi.Data.OrvosiDbContext();

            var target = await context.InvoiceDetails
                .Include(id => id.Invoice)
                .Include(id => id.ServiceRequest)
                .FirstAsync(id => id.Id == form.Id);

            target.Invoice.CustomerEmail = form.To;
            target.AdditionalNotes = form.AdditionalNotes;

            DateTime invoiceDate;
            DateTime.TryParseExact(form.InvoiceDate, "yyyy-MM-dd", null, DateTimeStyles.None, out invoiceDate);
            target.Invoice.InvoiceDate = invoiceDate;

            target.Amount = form.Amount;
            if ((target.ServiceRequest.IsNoShow || target.ServiceRequest.IsLateCancellation) && form.Rate != target.Rate && target.Rate.HasValue)
            {
                target.Rate = form.Rate;
                var discountType = target.ServiceRequest.GetDiscountType();
                target.DiscountDescription = InvoiceHelper.GetDiscountDescription(discountType, form.Rate, form.Amount);
            }
            target.CalculateTotal();
            target.AdditionalNotes = form.AdditionalNotes;

            target.Invoice.CalculateTotal();

            target.Invoice.ModifiedUser = User.Identity.Name;
            target.Invoice.ModifiedDate = SystemTime.Now();
            target.ModifiedUser = User.Identity.Name;
            target.ModifiedDate = SystemTime.Now();

            await context.SaveChangesAsync();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult Edit(int invoiceDetailId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var editForm = new Models.AccountingModel.Mapper(context).MapToEditForm(invoiceDetailId);
            
            return Json(editForm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceRequest(Guid? serviceProviderId, int serviceRequestId)
        {
            Guid userId = GetServiceProviderId(serviceProviderId);

            var context = new Orvosi.Data.OrvosiDbContext();
            var serviceRequests = new Models.AccountingModel.Mapper(context).MapToServiceRequest(userId, SystemTime.Now(), serviceRequestId);
            if (serviceRequests.Count() != 1)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return PartialView("_ServiceRequest", serviceRequests.First());
        }

        private Guid GetServiceProviderId(Guid? serviceProviderId)
        {
            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            return userId;
        }
    }
}