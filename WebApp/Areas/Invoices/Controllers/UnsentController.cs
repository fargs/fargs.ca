using Orvosi.Data;
using System;
using System.Data.Entity;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Areas.Invoices.Views.Unsent;
using WebApp.Areas.Shared;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Library.Helpers;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Invoices.Controllers
{
    public class UnsentController : BaseController
    {
        private DateTime _selectedDate;
        private OrvosiDbContext db;

        public UnsentController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }

        [AuthorizeRole(Feature = Features.Accounting.ViewUnsentInvoices)]
        public ActionResult Index(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            // calendar navigation component
            var calendarNavigation = new CalendarNavigationViewModel(_selectedDate, db, this.Request, identity, now);

            // unsent component
            var unsent = new UnsentViewModel(db, _selectedDate, identity, now);

            // this view model
            var viewModel = new IndexViewModel(calendarNavigation, unsent, identity, now);

            return View(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Accounting.ViewUnsentInvoices)]
        public PartialViewResult CalendarNavigation(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            var viewModel = new CalendarNavigationViewModel(_selectedDate, db, Request, identity, now);

            return PartialView(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Accounting.ViewUnsentInvoices)]
        public PartialViewResult Unsent(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);

            var viewModel = new UnsentViewModel(db, _selectedDate, identity, now);

            return PartialView(viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Accounting.ViewInvoice)]
        public PartialViewResult UnsentInvoice(int? invoiceId, int? serviceRequestId)
        {
            var viewModel = new UnsentInvoiceViewModel(db, serviceRequestId, invoiceId, identity, now);

            return PartialView("UnsentInvoice", viewModel);
        }
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Accounting.SendInvoice)]
        public async Task<PartialViewResult> EditAndSendInvoice(int invoiceId, int? serviceRequestId)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.Id == invoiceId);

            var message = BuildSendInvoiceMailMessage(invoice, Request.GetBaseUrl());
            var viewModel = new WebApp.ViewModels.MailMessageViewModel
            {
                InvoiceId = invoiceId,
                ServiceRequestId = serviceRequestId,
                Message = message
            };
            return PartialView("EditAndSendInvoice", viewModel);
        }
        private MailMessage BuildSendInvoiceMailMessage(Orvosi.Data.Invoice invoice, string baseUrl)
        {
            var message = new MailMessage();
            message.To.Add(invoice.CustomerEmail);
            message.From = new MailAddress(invoice.ServiceProviderEmail);
            message.Subject = string.Format("Invoice {0} - {1} - Payment Due {2}", invoice.InvoiceNumber, invoice.ServiceProviderName, invoice.DueDate.Value.ToOrvosiDateFormat());
            message.IsBodyHtml = true;
            message.Bcc.Add("lfarago@orvosi.ca,afarago@orvosi.ca");

            ViewData["BaseUrl"] = baseUrl; //This is needed because the full address needs to be included in the email download link
            message.Body = HtmlHelpers.RenderPartialViewToString(this, "~/Views/Shared/NotificationTemplates/InvoiceDownloadLink.cshtml", invoice);

            return message;
        }
    }
}