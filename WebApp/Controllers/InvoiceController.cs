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
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        private OrvosiEntities db = new OrvosiEntities();

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
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            // Service provider dropdown is defaulted to the current user unless you are a Super Admin.
            if (user.RoleId != Roles.SuperAdmin)
            {
                args.ServiceProviderId = new Guid(user.Id);
            }

            var now = SystemTime.Now();

            var query = db.Invoices
                .Where(i => !i.InvoiceDetails.All(id => id.ServiceRequest.CancelledDate.HasValue && id.ServiceRequest.IsLateCancellation == false))
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

            var dates = dateRange.Select(r => new { r.Month, r.Date});

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

            var netIncomeByCompany = invoiceTotals
                .Join(db.BillableEntities,
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

            vm.User = user;
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

        [Authorize(Roles = "Super Admin,Case Coordinator,Physician")]
        public ActionResult Table(List<Invoice> invoices)
        {
            return PartialView("_Table", invoices);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator")]
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

                var invoiceDate = SystemTime.Now();
                if (serviceRequest.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
                {
                    invoiceDate = serviceRequest.AppointmentDate.Value;
                }

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
                invoice.ApplyHst();
                invoice.CalculateTotal();
                db.Invoices.Add(invoice);

                if (db.GetValidationErrors().Count() == 0)
                {

                    await db.SaveChangesAsync();
                    var invoices = new List<Invoice>();
                    invoices.Add(invoice);
                    return PartialView("_Table", invoices);
                }
                return PartialView("_ValidationErrors", db.GetValidationErrors().First().ValidationErrors);
            }
            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);
        }

        [Authorize(Roles = "Super Admin, Case Coordinator, Physician")]
        public async Task<ActionResult> Details(int id)
        {
            ViewBag.User = await db.Users.SingleAsync(c => c.UserName == User.Identity.Name);
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

            invoice.ApplyHst();
            invoice.CalculateTotal();

            invoice.ModifiedUser = User.Identity.Name;
            invoiceDetail.ModifiedUser = User.Identity.Name;

            await db.SaveChangesAsync();

            //// Confirm the examination was completed.
            //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

            return RedirectToAction("Details", new { id = invoice.Id });
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
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.ObjectGuid == id);
            return PartialView("DocumentHeader", invoice);
        }

        [AllowAnonymous]
        public async Task<ActionResult> DownloadBody(Guid id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.ObjectGuid == id);
            return PartialView("PrintableInvoice", invoice);
        }

        [AllowAnonymous]
        public async Task<ActionResult> DownloadFooter(Guid id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.ObjectGuid == id);
            return PartialView("DocumentFooter", invoice);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Download(Guid id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.ObjectGuid == id);

            //var header = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentHeader", invoice);
            //var footer = HtmlHelpers.RenderViewToString(this.ControllerContext, "DocumentFooter", invoice);
            //var body = HtmlHelpers.RenderViewToString(this.ControllerContext, "PrintableInvoice", invoice);

            string apiKey = "eedbadc1-0fe7-4712-8573-816115379e62";
            using (var client = new WebClient())
            {
                NameValueCollection options = new NameValueCollection();
                options.Add("apikey", apiKey);
                options.Add("value", "http://orvosi.ca/invoice/downloadbody/" + id.ToString());

                options.Add("HeaderUrl", "http://orvosi.ca/invoice/downloadheader/" + id.ToString());
                options.Add("FooterUrl", "http://orvosi.ca/invoice/downloadfooter/" + id.ToString());

                options.Add("MarginTop", "40");
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
        //    //var intakeInterviewTask = db.ServiceRequestTasks.SingleOrDefault(c => c.ServiceRequestId == id && c.TaskId == Model.Enums.Tasks.IntakeInterview);

        //    return File(docBytes, "application/pdf", fileName);
        //}

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> Submit(int id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.Id == id);

            //var messageService = new MessagingService(Server.MapPath("~/Views/Shared/NotificationTemplates/"), null);
            //await messageService.SendInvoice(invoice, Request.GetBaseUrl());

            invoice.SentDate = SystemTime.Now();
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;

            await db.SaveChangesAsync();

            foreach (var item in invoice.InvoiceDetails)
            {
                var task = await db.ServiceRequestTasks.SingleOrDefaultAsync(c => c.TaskId == Tasks.SubmitInvoice && c.ServiceRequestId == item.ServiceRequestId && !c.IsObsolete);
                task.CompletedDate = SystemTime.Now();
                await db.SaveChangesAsync();
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> Unsubmit(int id)
        {
            var invoice = await db.Invoices.SingleOrDefaultAsync(c => c.Id == id);
            
            invoice.SentDate = null;
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = User.Identity.Name;

            await db.SaveChangesAsync();

            foreach (var item in invoice.InvoiceDetails)
            {
                var task = await db.ServiceRequestTasks.SingleOrDefaultAsync(c => c.TaskId == Tasks.SubmitInvoice && c.ServiceRequestId == item.ServiceRequestId && !c.IsObsolete);
                task.CompletedDate = null;
                await db.SaveChangesAsync();
            }

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