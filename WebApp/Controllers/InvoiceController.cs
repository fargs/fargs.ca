﻿using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Library.Enums;
using WebApp.Library;
using System.Data.Entity;
using WebApp.ViewModels.InvoiceViewModels;
using Orvosi.Shared.Enums;
using System.Globalization;
using WebApp.Library.Extensions;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using WebApp.Library.Helpers;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private OrvosiDbContext db = new OrvosiDbContext();
        
        public InvoiceController()
        {
            var serviceRequestController = new ServiceRequestController();
        }

        internal void NoShowToggled(object sender, EventArgs e)
        {
            Console.WriteLine("Event handled in Invoice Controller");
        }

        [Authorize(Roles = "Super Admin,Case Coordinator,Physician")]
        public async Task<ActionResult> Dashboard(FilterArgs args)
        {
            // Service provider dropdown is defaulted to the current user unless you are a Super Admin.
            if (User.Identity.GetRoleId() != AspNetRoles.SuperAdmin)
            {
                args.ServiceProviderId = User.Identity.GetGuidUserId();
            }

            var now = SystemTime.Now();

            var query = db.Invoices
                .Where(i => !i.InvoiceDetails.All(id => id.ServiceRequest.CancelledDate.HasValue ))
                .Where(i => i.InvoiceDate < now)
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

            var dates = dateRange.Select(r => new { r.Month, r.Date });

            var invoiceTotals = invoices
                .Select(i => new
                {
                    i.CustomerGuid,
                    i.ServiceProviderName,
                    i.InvoiceDate,
                    i.Total,
                    i.SubTotal,
                    Hst = i.Total - i.SubTotal
                });

            var netIncomeByMonth = dateRange
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
                    Year = c.Key.Year,
                    Month = c.Key.Month,
                    Hst = c.Sum(s => s.Hst),
                    SubTotal = c.Sum(s => s.SubTotal - (s.SubTotal * (decimal?)0.35)),
                    Expenses = c.Sum(s => s.SubTotal * (decimal?)0.35)
                });
            var billableEntities = db.BillableEntities.Select(be => new { be.EntityGuid, be.EntityName });
            var netIncomeByCompany = invoiceTotals
                .Join(billableEntities,
                    i => i.CustomerGuid,
                    c => c.EntityGuid,
                    (i, c) => new
                    {
                        EntityGuid = c.EntityGuid,
                        CompanyName = c.EntityName,
                        Hst = i.Hst,
                        SubTotal = i.SubTotal
                    })
                .GroupBy(c => new { c.EntityGuid, c.CompanyName })
                .Select(c => new
                {
                    CompanyName = c.Key.CompanyName,
                    Hst = c.Sum(i => i.Hst),
                    SubTotal = c.Sum(s => s.SubTotal - (s.SubTotal * (decimal?)0.35)),
                    Expenses = c.Sum(s => s.SubTotal * (decimal?)0.35)
                });

            var vm = new DashboardViewModel();

            vm.Months = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            vm.Companies = netIncomeByCompany.Select(c => c.CompanyName).Distinct();
            vm.NetIncomeByMonth = netIncomeByMonth.Select(c => c.SubTotal);
            vm.NetIncomeByCompany = netIncomeByCompany.Select(c => c.SubTotal);
            vm.NetIncome = netIncomeByMonth.Sum(c => c.SubTotal);
            vm.Hst = netIncomeByMonth.Sum(c => c.Hst);
            vm.Expenses = netIncomeByMonth.Sum(c => c.Expenses);
            vm.InvoiceCount = invoiceTotals.Count();
            vm.Invoices = invoices;
            vm.FilterArgs = args;

            return View(vm);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        public async Task<ActionResult> Index(FilterArgs args)
        {
            // Service provider dropdown is defaulted to the current user unless you are a Super Admin.
            if (User.Identity.GetRoleId() != AspNetRoles.SuperAdmin)
            {
                args.ServiceProviderId = User.Identity.GetGuidUserId();
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

            if (!args.ShowSubmitted)
            {
                query = query.Where(i => !i.SentDate.HasValue);
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

            vm.Invoices = invoices;

            return View(vm);
        }

        [Authorize(Roles = "Super Admin,Case Coordinator,Physician")]
        public ActionResult Table(List<Invoice> invoices)
        {
            return PartialView("_Table", invoices);
        }

        public async Task<ActionResult> Create(int serviceRequestId)
        {
            var sr = await db.ServiceRequests.FindAsync(serviceRequestId);
            var invoice = new Invoice();

            return View();
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var serviceRequestId = int.Parse(this.Request.Form.Get("Id"));
            //var serviceRequest = await db.ServiceRequestViews.FindAsync(serviceRequestId);

            var serviceRequest =
                db.ServiceRequests
                    //.Include(sr => sr.Company.Parent)
                    //.Include(sr => sr.Physician.AspNetUser)
                    //.Include(sr => sr.Service.ServiceCategory)
                    //.Include(sr => sr.InvoiceDetails)
                    //.Include(sr => sr.CaseCoordinator)
                    //.Include(sr => sr.DocumentReviewer)
                    //.Include(sr => sr.IntakeAssistant)
                    //.Include(sr => sr.Address.Province)
                    .Find(serviceRequestId);

            // check if the no show rates are set in the request. Migrate old records to use invoices.
            if (!serviceRequest.NoShowRate.HasValue || !serviceRequest.LateCancellationRate.HasValue)
            {
                var rates = db.GetServiceCatalogueRate(serviceRequest.PhysicianId, serviceRequest.Company.ObjectGuid).First();
                serviceRequest.NoShowRate = rates.NoShowRate;
                serviceRequest.LateCancellationRate = rates.LateCancellationRate;
            }

            var serviceProvider = db.BillableEntities.First(c => c.EntityGuid == serviceRequest.PhysicianId);
            var customer = db.BillableEntities.First(c => c.EntityGuid == serviceRequest.Company.ObjectGuid);

            var invoiceNumber = db.GetNextInvoiceNumber().First();

            var invoiceDate = SystemTime.Now();
            if (serviceRequest.Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
            {
                invoiceDate = serviceRequest.AppointmentDate.Value;
            }

            var invoice = new Invoice();
            invoice.BuildInvoice(serviceProvider, customer, invoiceNumber.NextInvoiceNumber, invoiceDate, User.Identity.Name);

            // Create or update the invoice detail for the service request
            InvoiceDetail invoiceDetail;

            //invoiceDetail = db.InvoiceDetails.SingleOrDefault(c => c.ServiceRequestId == serviceRequest.Id);
            //if (invoiceDetail == null)
            //{
                invoiceDetail = new InvoiceDetail();
                invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
                invoice.InvoiceDetails.Add(invoiceDetail);
            //}
            //else
            //{
            //    invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
            //}
            invoice.CalculateTotal();
            db.Invoices.Add(invoice);

            if (db.GetValidationErrors().Count() == 0)
            {

                await db.SaveChangesAsync();
                var invoices = new List<Invoice>();
                invoices.Add(invoice);
                return RedirectToAction("Details", "ServiceRequest", new { id = serviceRequestId });
            }
            return PartialView("_ValidationErrors", db.GetValidationErrors().First().ValidationErrors);

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Enums.Tasks.IntakeInterview);
        }

        [ChildActionOnly]
        public ActionResult ReadOnly(Orvosi.Data.Invoice invoice)
        {
            return PartialView("_ReadOnly", invoice);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator, Physician")]
        public async Task<ActionResult> Details(int id)
        {
            var user = await db.AspNetUsers.FirstAsync(c => c.UserName == User.Identity.Name);
            ViewBag.RoleId = user.AspNetUserRoles.First().RoleId;
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.ReadOnly;
            return View(obj);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        public async Task<ActionResult> Edit(int id)
        {
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.Edit;
            return View(obj);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        [HttpPost]
        public async Task<ActionResult> Edit(InvoiceDetail updatedInvoiceDetail)
        {
            var id = int.Parse(Request.Form.Get("Id"));
            DateTime invoiceDate;
            DateTime.TryParseExact(Request.Form.Get("InvoiceDate"), "yyyy-MM-dd", null, DateTimeStyles.None, out invoiceDate);

            // Here we should parse out all of the invoice details and loop through them. For now we know there is only going to be one.
            var invoiceDetail = await db.InvoiceDetails.FindAsync(id);
            var invoice = invoiceDetail.Invoice;
            invoice.InvoiceDate = invoiceDate;

            invoiceDetail.Description = updatedInvoiceDetail.Description;
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
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Enums.Tasks.IntakeInterview);

            var button = Request.Form.Get("button");
            if (button == "SaveAndSubmit")
            {
                return RedirectToAction("Details", "ServiceRequest", new { id = invoice.InvoiceDetails.First().ServiceRequest.Id });
            }
            else
            {
                return RedirectToAction("Details", new { id = invoice.Id });
            }
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        public ActionResult Delete(int invoiceDetailId)
        {
            var invoiceDetail = db.InvoiceDetails.Find(invoiceDetailId);
            invoiceDetail.IsDeleted = true;
            invoiceDetail.DeletedBy = User.Identity.GetGuidUserId();
            invoiceDetail.DeletedDate = SystemTime.Now();

            var invoice = invoiceDetail.Invoice;
            invoice.IsDeleted = true;
            invoice.DeletedBy = User.Identity.GetGuidUserId();
            invoice.DeletedDate = SystemTime.Now();

            db.SaveChanges();
            return RedirectToAction("Details", "ServiceRequest", new { id = invoiceDetail.ServiceRequestId });
        }

        //[AllowAnonymous]
        //public ActionResult DownloadReport(Guid id)
        //{
        //    var invoice = db.Invoices.Where(c => c.ObjectGuid == id);
        //    if (invoice.FirstOrDefault() == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //    var invoiceDetails = db.InvoiceDetails.Where(c => c.InvoiceId == invoice.FirstOrDefault().Id);
        //    var person = db.Users.SingleOrDefault(c => c.UserName == User.Identity.Name);

        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath("~/Content/reports/Invoice.rdlc");
        //    var invoiceDataSet = new ReportDataSource("Invoice", invoice);
        //    var invoiceDetailDataSet = new ReportDataSource("InvoiceDetail", invoiceDetails);

        //    localReport.DataSources.Add(invoiceDataSet);
        //    localReport.DataSources.Add(invoiceDetailDataSet);
        //    string reportType = "PDF";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    //The DeviceInfo settings should be changed based on the reportType
        //    //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>PDF</OutputFormat>" +
        //    "  <PageWidth>11in</PageWidth>" +
        //    "  <PageHeight>8.5in</PageHeight>" +
        //    "  <MarginTop>0.5in</MarginTop>" +
        //    "  <MarginLeft>1in</MarginLeft>" +
        //    "  <MarginRight>1in</MarginRight>" +
        //    "  <MarginBottom>0.5in</MarginBottom>" +
        //    "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    //Render the report
        //    renderedBytes = localReport.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);
        //    //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);
        //    return File(renderedBytes, mimeType);
        //}

        [AllowAnonymous]
        public async Task<ActionResult> DownloadHeader(Guid id)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.ObjectGuid == id);
            return PartialView("DocumentHeader", invoice);
        }

        [AllowAnonymous]
        public async Task<ActionResult> DownloadBody(Guid id)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.ObjectGuid == id);
            return PartialView("PrintableInvoice", invoice);
        }

        [AllowAnonymous]
        public async Task<ActionResult> DownloadFooter(Guid id)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.ObjectGuid == id);
            return PartialView("DocumentFooter", invoice);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Download(Guid id)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.ObjectGuid == id);

            //var header = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentHeader", invoice);
            //var footer = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentFooter", invoice);
            var body = HtmlHelpers.RenderViewToString(this.ControllerContext, "PrintableInvoice", invoice);

            string apiKey = "eedbadc1-0fe7-4712-8573-816115379e62";
            using (var client = new WebClient())
            {
                NameValueCollection options = new NameValueCollection();
                options.Add("apikey", apiKey);
                //options.Add("value", "https://orvosi.ca/invoice/downloadbody/" + id.ToString());
                options.Add("value", body);
                //options.Add("HeaderUrl", "https://orvosi.ca/invoice/downloadheader/" + id.ToString());
                //options.Add("FooterUrl", "https://orvosi.ca/invoice/downloadfooter/" + id.ToString());

                options.Add("MarginTop", "10");
                options.Add("MarginBottom", "10");
                options.Add("MarginLeft", "10");
                options.Add("MarginRight", "10");

                // Call the API convert to a PDF
                MemoryStream ms = new MemoryStream(client.UploadValues("http://api.html2pdfrocket.com/pdf", options));

                // Make the file a downloadable attachment - comment this out to show it directly inside
                HttpContext.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.pdf", invoice.ServiceProviderName + "_" + invoice.InvoiceNumber));

                // Return the file as a PDF
                return new FileStreamResult(ms, "application/pdf");
            }
        }

        //[AllowAnonymous]
        //public async Task<ActionResult> Download(Guid id)
        //{
        //    var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.ObjectGuid == id);

        //    var header = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentHeader", invoice);
        //    var footer = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentFooter", invoice);
        //    var body = HtmlHelpers.RenderViewToString(this.ControllerContext, "PrintableInvoice", invoice);

        //    HtmlToPdf converter = new HtmlToPdf();

        //    // header settings
        //    converter.Options.DisplayHeader = true;
        //    converter.Header.Height = 100;

        //    PdfHtmlSection headerHtml = new PdfHtmlSection(header, Request.GetBaseUrl());
        //    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //    converter.Header.Add(headerHtml);

        //    // footer settings
        //    converter.Options.DisplayFooter = true;
        //    converter.Footer.Height = 30;

        //    PdfHtmlSection footerHtml = new PdfHtmlSection(footer, Request.GetBaseUrl());
        //    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //    converter.Footer.Add(footerHtml);

        //    PdfTextSection text = new PdfTextSection(0, 10,
        //            "Page {page_number} of {total_pages}  ",
        //            new System.Drawing.Font("Arial", 8));
        //    text.HorizontalAlign = PdfTextHorizontalAlign.Right;
        //    converter.Footer.Add(text);

        //    converter.Options.PdfPageSize = PdfPageSize.A4;
        //    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
        //    converter.Options.MarginLeft = 30;
        //    converter.Options.MarginRight = 30;
        //    converter.Options.MarginTop = 30;
        //    converter.Options.MarginBottom = 30;

        //    PdfDocument doc = converter.ConvertHtmlString(body, Request.GetBaseUrl());
        //    var fileName = string.Format(@"Invoice_{0}_{1}_{2}.pdf", invoice.InvoiceNumber, invoice.ServiceProviderName, invoice.CustomerName);
        //    var docBytes = doc.Save();
        //    doc.Close();

        //    invoice.DownloadDate = SystemTime.Now();
        //    invoice.ModifiedUser = User.Identity.Name;
        //    await db.SaveChangesAsync();

        //    //// Confirm the examination was completed.
        //    //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Enums.Tasks.IntakeInterview);

        //    return File(docBytes, "application/pdf", fileName);
        //}

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> Submit(int id)
        {

            var _invoice = await db.Invoices.FirstAsync(c => c.Id == id);

            //var messageService = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), null);
            //await messageService.SendInvoice(invoice, Request.GetBaseUrl());

            _invoice.SentDate = SystemTime.Now();
            _invoice.ModifiedDate = SystemTime.Now();
            _invoice.ModifiedUser = User.Identity.Name;

            foreach (var item in _invoice.InvoiceDetails)
            {
                var task = item.ServiceRequest.ServiceRequestTasks.FirstOrDefault(c => c.TaskId == Tasks.SubmitInvoice || c.TaskId == 37); // TODO: 37 is submit invoice for add/pr
                task.CompletedDate = SystemTime.Now();
            }
            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> Unsubmit(int id)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.Id == id);

            invoice.SentDate = null;
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;

            foreach (var item in invoice.InvoiceDetails)
            {
                var task = item.ServiceRequest.ServiceRequestTasks.FirstOrDefault(c => c.TaskId == Tasks.SubmitInvoice || c.TaskId == 37); // TODO: 37 is submit invoice for add/pr
                task.CompletedDate = null;
            }
            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        [HttpPost]
        public async Task<ActionResult> AddInvoiceDetail(InvoiceDetail InvoiceDetail)
        {
            throw new NotImplementedException();
            db.InvoiceDetails.Add(InvoiceDetail);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", new { id = InvoiceDetail.InvoiceId });
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
        [HttpPost]
        public async Task<ActionResult> EditInvoiceDetail(int id)
        {
            throw new NotImplementedException();
            var obj = await db.Invoices.FindAsync(id);
            ViewBag.FormMode = FormModes.Edit;
            return View(obj);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
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