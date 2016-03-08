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
using WebApp.Library.Extensions;
using SelectPdf;
using WebApp.Library.Helpers;

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

        public ActionResult Table(List<Invoice> invoices)
        {
            return PartialView("_Table", invoices);
        }

        [HttpPost]
        public async Task<ActionResult> Create(short ServiceRequestId)
        {
            using (var context = new OrvosiEntities(User.Identity.Name))
            {
                var serviceRequest = await db.ServiceRequests.FindAsync(ServiceRequestId);

                var serviceProvider = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
                var customer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

                var invoiceNumber = db.GetNextInvoiceNumber().SingleOrDefault();

                var service = new InvoiceService(User.Identity.Name);
                var invoice = service.BuildInvoice(invoiceNumber, serviceProvider, customer, serviceRequest, User.Identity.Name);
                db.Invoices.Add(invoice);
                if (db.GetValidationErrors().Count() == 0)
                {
                    await db.SaveChangesAsync();
                    return PartialView("_InvoiceTable", invoice.InvoiceDetails.ToList());
                }
                return PartialView("_ValidationErrors", db.GetValidationErrors().First().ValidationErrors);
            }
            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);
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
            return View(obj);
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

            if (string.IsNullOrEmpty(Request.UrlReferrer.ToString()))
            {
                return RedirectToAction("Details", new { id = invoice.Id });
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [AllowAnonymous]
        public async Task<ActionResult> Download(Guid id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.ObjectGuid == id);

            var header = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentHeader", invoice);
            var footer = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentFooter", invoice);
            var body = HtmlHelpers.RenderViewToString(this.ControllerContext, "PrintableInvoice", invoice);
            
            HtmlToPdf converter = new HtmlToPdf();

            // header settings
            converter.Options.DisplayHeader = true;
            converter.Header.Height = 100;

            PdfHtmlSection headerHtml = new PdfHtmlSection(header, Request.GetBaseUrl());
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 30;

            PdfHtmlSection footerHtml = new PdfHtmlSection(footer, Request.GetBaseUrl());
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            PdfTextSection text = new PdfTextSection(0, 10,
                    "Page {page_number} of {total_pages}  ",
                    new System.Drawing.Font("Arial", 8));
            text.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(text);

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 30;
            converter.Options.MarginRight = 30;
            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 30;
            
            PdfDocument doc = converter.ConvertHtmlString(body, Request.GetBaseUrl());
            var filePath = string.Format(@"{0}\App_Data\Invoice_{1}.pdf", Server.MapPath("~"), invoice.InvoiceNumber);
            var docBytes = doc.Save();
            doc.Close();

            invoice.WasDownloaded = true;
            invoice.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return File(docBytes, "application/pdf", string.Format("Invoice_{0}.pdf", invoice.InvoiceNumber));
        }


        public async Task<ActionResult> Submit(int id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.Id == id);

            var messageService = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), null);
            await messageService.SendInvoice(invoice, Request.GetBaseUrl());

            invoice.SentDate = SystemTime.Now();
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();
            return PartialView("_InvoiceSubmitted", invoice.SentDate.Value);
        }

        [HttpPost]
        public async Task<ActionResult> AddInvoiceDetail(InvoiceDetail InvoiceDetail)
        {
            throw new NotImplementedException();
            db.InvoiceDetails.Add(InvoiceDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = InvoiceDetail.InvoiceId });
        }

        [HttpPost]
        public async Task<ActionResult> EditInvoiceDetail(int id)
        {
            throw new NotImplementedException();
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.Edit;
            return View(obj);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteInvoiceDetail(int id)
        {
            throw new NotImplementedException();
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