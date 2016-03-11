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
    [Authorize(Roles = "Super Admin, Case Coordinator")]
    public class InvoiceController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

        public InvoiceController()
        {
            var serviceRequestController = new ServiceRequestController();
            serviceRequestController.NoShowToggledEvent += new NoShowToggledHandler(NoShowToggled);
        }

        internal void NoShowToggled(object sender, EventArgs e)
        {
            Console.WriteLine("Event handled in Invoice Controller");
        }


        public async Task<ActionResult> Dashboard(FilterArgs args)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            // Service provider dropdown is defaulted to the current user unless you are a Super Admin.
            if (user.RoleId != Roles.SuperAdmin)
            {
                args.ServiceProviderId = new Guid(user.Id);
            }

            var query = db.Invoices
                .AsQueryable();

            // Apply the service provider and customer filters.
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

            args.Year = args.Year.HasValue ? args.Year.Value : DateTime.Today.Year;
            // Apply the year and month filters.
            query = query.Where(c => c.InvoiceDate.Year == args.Year);

            var invoices = await query.ToListAsync();

            var startDate = new DateTime(args.Year.Value, 01, 01);
            var endDate = startDate.AddYears(1);
            var dateRange = startDate.GetDateRangeTo(endDate);

            var dates = dateRange.Select(r => new { r.Month, r.Date});

            var invoiceTotals = invoices
                .Where(i => i.InvoiceDate > startDate && i.InvoiceDate <= endDate)
                .Select(i => new
                {
                    i.ServiceProviderName,
                    i.InvoiceDate,
                    i.Total,
                    i.SubTotal,
                    Hst = i.Total - i.SubTotal
                })
                .ToList();

            var summary = dateRange
                .GroupJoin(invoiceTotals,
                    r => r.Date,
                    t => t.InvoiceDate,
                    (r, t) => new
                    {
                        Date = r,
                        Hst = t.Sum(c => c.Hst),
                        SubTotal = t.Sum(c => c.SubTotal),
                    })
                .GroupBy(c => new { c.Date.Month, c.Date.Year })
                .Select(c => new
                {
                    Hst = c.Sum(s => s.Hst),
                    SubTotal = c.Sum(s => s.SubTotal - (s.SubTotal * (decimal?)0.35)),
                    Expenses = c.Sum(s => s.SubTotal * (decimal?)0.35)
                });

            var vm = new DashboardViewModel();

            vm.User = user;
            vm.SubTotal = summary.Select(c => c.SubTotal);
            vm.Hst = summary.Select(c => c.Hst);
            vm.Expenses = summary.Select(c => c.Expenses);
            vm.FilterArgs = args;

            return View(vm);
        }

        public async Task<ActionResult> Index(FilterArgs args)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            // Service provider dropdown is defaulted to the current user unless you are a Super Admin.
            if (user.RoleId != Roles.SuperAdmin)
            { 
                args.ServiceProviderId = new Guid(user.Id);
            }

            var query = db.Invoices
                .AsQueryable();

            // Apply the service provider and customer filters.
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

            // Apply the year and month filters.
            args.Year = args.Year.HasValue ? args.Year.Value : SystemTime.Now().Year;
            query = query.Where(c => c.InvoiceDate.Year == args.Year.Value);
            if (args.Month.HasValue)
            {
                query = query.Where(c => c.InvoiceDate.Month == args.Month);
            }

            var invoices = await query.ToListAsync();

            //var thisMonth = new DateTime(SystemTime.Now().Year, SystemTime.Now().Month, 1);
            //var nextMonth = thisMonth.AddMonths(1);
            //if (args.Year.HasValue && args.Month.HasValue)
            //{
            //    thisMonth = new DateTime(args.Year.Value, args.Month.Value, 1);
            //    nextMonth = thisMonth.AddMonths(1);
            //}
            //args.Year = thisMonth.Year;
            //args.Month = thisMonth.Month;
            //args.FilterDate = thisMonth;

            var vm = new IndexViewModel();

            vm.FilterArgs = args;

            vm.CurrentUser = db.Users.Single(u => u.UserName == User.Identity.Name);

            vm.Invoices = invoices;
            
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

                // check if the no show rates are set in the request. Migrate old records to use invoices.
                if (!serviceRequest.NoShowRate.HasValue || !serviceRequest.LateCancellationRate.HasValue)
                {
                    var rates = db.GetServiceCatalogueRate(new Guid(serviceRequest.PhysicianId), serviceRequest.CompanyGuid).First();
                    serviceRequest.NoShowRate = rates.NoShowRate;
                    serviceRequest.LateCancellationRate = rates.LateCancellationRate;
                }

                var serviceProvider = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid.ToString() == serviceRequest.PhysicianId);
                var customer = await db.BillableEntities.SingleOrDefaultAsync(c => c.EntityGuid == serviceRequest.CompanyGuid.Value);

                var invoiceNumber = db.GetNextInvoiceNumber().SingleOrDefault();
                var invoiceDate = serviceRequest.AppointmentDate.Value;

                var invoice = new Invoice();
                invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, User.Identity.Name);

                // Create or update the invoice detail for the service request
                InvoiceDetail invoiceDetail;

                invoiceDetail = db.InvoiceDetails.SingleOrDefault(c => c.ServiceRequestId == serviceRequest.Id);
                if (invoiceDetail == null)
                {
                    invoiceDetail = new InvoiceDetail();
                    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
                    invoice.InvoiceDetails.Add(invoiceDetail);
                }
                else
                {
                    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
                }

                invoice.CalculateTotal();
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

            // Here we should parse out all of the invoice details and loop through them. For now we know there is only going to be one.
            var invoiceDetail = await db.InvoiceDetails.FindAsync(id);
            var invoice = invoiceDetail.Invoice;

            invoiceDetail.Amount = updatedInvoiceDetail.Amount;
            if (!string.IsNullOrEmpty(invoiceDetail.DiscountDescription))
            {

                var discountType = invoiceDetail.ServiceRequest.GetDiscountType();
                invoiceDetail.DiscountDescription = InvoiceHelper.GetDiscountDescription(discountType, invoiceDetail.Rate, invoiceDetail.Amount);
            }
            invoiceDetail.CalculateTotal();
            invoiceDetail.AdditionalNotes = updatedInvoiceDetail.AdditionalNotes;

            invoice.CalculateTotal();

            invoice.ModifiedUser = User.Identity.Name;
            invoiceDetail.ModifiedUser = User.Identity.Name;

            await db.SaveChangesAsync();

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return RedirectToAction("Details", new { id = invoice.Id });
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
            var fileName = string.Format(@"Invoice_{0}_{1}_{2}.pdf", invoice.InvoiceNumber, invoice.ServiceProviderName, invoice.CustomerName);
            var docBytes = doc.Save();
            doc.Close();

            invoice.DownloadDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;
            await db.SaveChangesAsync();

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return File(docBytes, "application/pdf", fileName);
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

            foreach (var item in invoice.InvoiceDetails)
            {
                var task = await db.ServiceRequestTasks.SingleOrDefaultAsync(c => c.TaskId == Tasks.SubmitInvoice && c.ServiceRequestId == item.ServiceRequestId);
                task.CompletedDate = SystemTime.Now();
                await db.SaveChangesAsync();
            }

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