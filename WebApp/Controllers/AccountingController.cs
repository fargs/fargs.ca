using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using System.Linq.Expressions;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Library.Projections;
using MoreLinq;
using WebApp.Library.Filters;
using Orvosi.Shared.Enums.Features;
using System.Text;
using WebApp.Models;
using LinqKit;
using WebApp.ViewModels.CalendarViewModels;
using WebApp.ViewModels;
using WebApp.Library.Helpers;
using System.Collections.Specialized;
using Orvosi.Data.Extensions;
using System.Security.Principal;

namespace WebApp.Controllers
{
    public class AccountingController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService workService;
        private AccountingService accountingService;

        public AccountingController(OrvosiDbContext db, WorkService workService, AccountingService accountingService, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.workService = workService;
            this.accountingService = accountingService;
        }

        [AuthorizeRole(Feature = Accounting.ViewUnsentInvoices)]
        public ActionResult UnsentInvoices(FilterArgs filterArgs)
        {
            filterArgs.Year = filterArgs.Year ?? now.Year;
            filterArgs.Month = filterArgs.Month ?? now.Month;

            var serviceRequests = db.ServiceRequests
                .ForPhysician(physicianOrLoggedInUserId)
                .AreNotCancellations()
                .WhereSubmitInvoiceTaskIsToDo()
                .AreForCompany(filterArgs.CustomerId)
                .AreWithinDateRange(now, filterArgs.Year, filterArgs.Month)
                .Select(ServiceRequestDto.FromServiceRequestEntityForInvoice.Expand())
                .ToList();

            var invoices = db.Invoices
                .AreOwnedBy(physicianOrLoggedInUserId)
                .AreNotDeleted()
                .AreNotSent()
                .AreForCustomer(filterArgs.CustomerId)
                .AreWithinDateRange(now, filterArgs.Year, filterArgs.Month)
                .Select(InvoiceDto.FromInvoiceEntity.Expand())
                .ToList();

            // Full outer join on these 2 lists.
            var leftSide = from sr in serviceRequests
                           join i in invoices on sr.Id equals i.ServiceRequestId into g
                           from sub in g.DefaultIfEmpty()
                           select new UnsentInvoiceViewModel
                           {
                               ServiceRequest = CaseViewModel.FromServiceRequestDto.Invoke(sr),
                               Invoice = sub != null ? InvoiceViewModel.FromInvoiceDto.Invoke(sub) : null
                           };

            var rightSide = from i in invoices
                            where !i.ServiceRequestId.HasValue
                            select new UnsentInvoiceViewModel
                            {
                                Invoice = InvoiceViewModel.FromInvoiceDto.Invoke(i)
                            };

            var result = leftSide.Concat(rightSide).ToList();

            var model = result
                .GroupBy(d => new { Day = d.Day.Date })
                .Select(d => new DayViewModel
                {
                    Day = d.Key.Day,
                    UnsentInvoices = d
                        .OrderBy(sr => d.Key.Day)
                        .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                }).OrderBy(df => df.Day);

            var viewModel = new InvoiceListViewModel
            {
                FilterArgs = filterArgs,
                UnsentInvoices = model
            };

            return View("Invoices", viewModel);
        }

        [AuthorizeRole(Feature = Accounting.ViewUnpaidInvoices)]
        public ActionResult UnpaidInvoices(FilterArgs filterArgs)
        {
            filterArgs.Year = filterArgs.Year ?? now.Year;
            filterArgs.Month = filterArgs.Month ?? now.Month;

            var serviceRequests = db.ServiceRequests
                .ForPhysician(physicianOrLoggedInUserId)
                //.HaveCompletedSubmitInvoiceTask()
                //.AreForCompany(filterArgs.CustomerId)
                //.AreWithinDateRange(filterArgs.Year, filterArgs.Month)
                .Select(ServiceRequestDto.FromServiceRequestEntityForInvoice.Expand())
                .ToList();

            var invoices = db.Invoices
                .AreOwnedBy(physicianOrLoggedInUserId)
                .AreSent()
                .AreUnpaid()
                .AreForCustomer(filterArgs.CustomerId)
                .AreWithinDateRange(now, filterArgs.Year, filterArgs.Month)
                .Select(InvoiceDto.FromInvoiceEntity.Expand())
                .ToList();

            // Full outer join on these 2 lists.
            var leftSide = from i in invoices
                           join sr in serviceRequests on i.ServiceRequestId equals sr.Id into g
                           from sub in g.DefaultIfEmpty()
                           select new UnsentInvoiceViewModel
                           {
                               ServiceRequest = sub != null ? CaseViewModel.FromServiceRequestDto.Invoke(sub) : null,
                               Invoice = InvoiceViewModel.FromInvoiceDto.Invoke(i)
                           };

            var model = leftSide
                .GroupBy(d => new { Day = d.Invoice.InvoiceDate.Date }) // NOTE: Grouped by Invoice Date
                .Select(d => new DayViewModel
                {
                    Day = d.Key.Day,
                    UnsentInvoices = d
                        .OrderBy(sr => d.Key.Day)
                        .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                }).OrderBy(df => df.Day);

            var viewModel = new InvoiceListViewModel
            {
                FilterArgs = filterArgs,
                UnsentInvoices = model
            };

            return View("Invoices", viewModel);
        }

        [AuthorizeRole(Feature = Accounting.SearchInvoices)]
        public ActionResult AllInvoices(FilterArgs filterArgs)
        {
            filterArgs.Year = filterArgs.Year ?? now.Year;
            filterArgs.Month = filterArgs.Month ?? now.Month;

            var serviceRequests = db.ServiceRequests
                .ForPhysician(physicianOrLoggedInUserId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForInvoice.Expand())
                .ToList();

            // if the users searches for a specific invoice, only show that invoice.
            var query = db.Invoices
                .AreOwnedBy(physicianOrLoggedInUserId);

            if (filterArgs.InvoiceId.HasValue)
            {
                query = query
                    .WithId(filterArgs.InvoiceId.Value);
            }
            else
            {
                query = query
                    .AreOwnedBy(physicianOrLoggedInUserId)
                    .AreForCustomer(filterArgs.CustomerId)
                    .AreWithinDateRange(now, filterArgs.Year, filterArgs.Month);
            }

            var invoices = query
                .Select(InvoiceDto.FromInvoiceEntity.Expand())
                .ToList();

            // Full outer join on these 2 lists.
            var leftSide = from i in invoices
                           join sr in serviceRequests on i.ServiceRequestId equals sr.Id into g
                           from sub in g.DefaultIfEmpty()
                           select new UnsentInvoiceViewModel
                           {
                               ServiceRequest = sub != null ? CaseViewModel.FromServiceRequestDto.Invoke(sub) : null,
                               Invoice = InvoiceViewModel.FromInvoiceDto.Invoke(i)
                           };

            var model = leftSide
                .GroupBy(d => new { Day = d.Invoice.InvoiceDate.Date }) // NOTE: Grouped by Invoice Date
                .Select(d => new DayViewModel
                {
                    Day = d.Key.Day,
                    UnsentInvoices = d
                        .OrderBy(sr => d.Key.Day)
                        .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                }).OrderBy(df => df.Day);

            var viewModel = new InvoiceListViewModel
            {
                FilterArgs = filterArgs,
                UnsentInvoices = model
            };

            return View("Invoices", viewModel);
        }

        [AuthorizeRole(Feature = Accounting.Analytics)]
        public ActionResult Analytics(FilterArgs filterArgs)
        {
            var invoices = db.Invoices
                .AreOwnedBy(physicianOrLoggedInUserId)
                .AreNotDeleted()
                .AreSent()
                .AreForCustomer(filterArgs.CustomerId)
                .AreWithinDateRange(now, filterArgs.Year, filterArgs.Month)
                .Select(InvoiceProjections.Header())
                .ToList();

            filterArgs.Year = filterArgs.Year.GetValueOrDefault(now.Year);
            var startDate = filterArgs.Year.HasValue ? new DateTime(filterArgs.Year.Value, 01, 01) : new DateTime(now.Year, 1, 1);
            var endDate = startDate.AddYears(1);
            var dateRange = startDate.GetDateRangeTo(endDate);

            var dates = dateRange.Select(r => new { r.Month, r.Date });

            var invoiceTotals = invoices
                .Select(i => new
                {
                    CustomerId = i.Customer.Id,
                    ServiceProviderName = i.ServiceProvider.Name,
                    i.InvoiceDate,
                    i.Total,
                    i.SubTotal,
                    Hst = i.Hst
                });

            var netIncomeByMonth = dateRange
                .GroupJoin(invoiceTotals,
                    r => r.Date,
                    t => t.InvoiceDate,
                    (r, t) => new
                    {
                        Date = r,
                        Hst = t.Sum(c => c.Hst),
                        Total = t.Sum(c => c.Total),
                    })
                .GroupBy(c => new { c.Date.Month, c.Date.Year })
                .Select(c => new
                {
                    Year = c.Key.Year,
                    Month = c.Key.Month,
                    Hst = c.Sum(s => s.Hst),
                    Total = c.Sum(s => s.Total)
                });
            var billableEntities = db.BillableEntities.Select(be => new { be.EntityGuid, be.EntityName });
            var netIncomeByCompany = invoiceTotals
                .Join(billableEntities,
                    i => i.CustomerId,
                    c => c.EntityGuid,
                    (i, c) => new
                    {
                        EntityGuid = c.EntityGuid,
                        CompanyName = c.EntityName,
                        Hst = i.Hst,
                        Total = i.Total
                    })
                .GroupBy(c => new { c.EntityGuid, c.CompanyName })
                .Select(c => new
                {
                    CompanyName = c.Key.CompanyName,
                    Hst = c.Sum(i => i.Hst),
                    Total = c.Sum(s => s.Total)
                });

            var vm = new DashboardViewModel();

            vm.Months = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            vm.Companies = netIncomeByCompany.Select(c => c.CompanyName).Distinct().ToList();
            vm.NetIncomeByMonth = netIncomeByMonth.Select(c => c.Total).ToList();
            vm.NetIncomeByCompany = netIncomeByCompany.Select(c => c.Total).ToList();
            vm.NetIncome = netIncomeByMonth.Sum(c => c.Total);
            vm.Hst = netIncomeByMonth.Sum(c => c.Hst);
            vm.InvoiceCount = invoiceTotals.Count();
            vm.Invoices = invoices;
            vm.FilterArgs = filterArgs;

            return View("~/Views/Invoice/Dashboard.cshtml", vm);
        }

        [AuthorizeRole(Feature = Accounting.ViewInvoice)]
        public ActionResult ServiceRequest(Guid? serviceProviderId, int serviceRequestId)
        {
            var serviceRequests = db.ServiceRequests
                    .Where(s => s.Id == serviceRequestId)
                    .Select(ServiceRequestProjections.DetailsWithInvoices(physicianOrLoggedInUserId, now))
                    .ToList();
            if (serviceRequests.Count() != 1)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return PartialView("_ServiceRequest", serviceRequests.First());
        }

        [AuthorizeRole(Feature = Accounting.ViewInvoice)]
        public ActionResult Invoice(int invoiceId)
        {
            var dto = db.Invoices
                .WithId(invoiceId)
                .Select(InvoiceDto.FromInvoiceEntity.Expand())
                .SingleOrDefault();

            var viewModel = dto == null ? null : InvoiceViewModel.FromInvoiceDto.Invoke(dto);

            return PartialView("_Invoice", viewModel);
        }

        [AuthorizeRole(Feature = Accounting.ViewInvoice)]
        public ActionResult InvoiceMenu(int invoiceId, int? serviceRequestId)
        {

            CaseViewModel caseViewModel = null;
            if (serviceRequestId.HasValue)
            {
                var dto = db.ServiceRequests
                .WithId(serviceRequestId.Value)
                .Select(ServiceRequestDto.FromServiceRequestEntityForInvoiceDetail.Expand())
                .SingleOrDefault();

                caseViewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);
            }

            var invoice = db.Invoices
                .Where(i => i.Id == invoiceId)
                .Select(InvoiceDto.FromInvoiceEntityForInvoiceMenu.Expand())
                .SingleOrDefault();

            var invoiceViewModel = invoice == null ? null : InvoiceViewModel.FromInvoiceDtoForInvoiceMenu.Invoke(invoice);

            var unsentInvoice = new UnsentInvoiceViewModel
            {
                ServiceRequest = caseViewModel,
                Invoice = invoiceViewModel
            };

            return PartialView("_InvoiceMenu", unsentInvoice);
        }

        [AuthorizeRole(Feature = Accounting.SearchInvoices)]
        public ActionResult Search(Guid? serviceProviderId, int? invoiceId, string searchTerm, int? page)
        {
            var data = db.Invoices
                .AreOwnedBy(physicianOrLoggedInUserId)
                .Where(i => i.InvoiceNumber.Contains(searchTerm)
                    || i.InvoiceDetails.Any(id => id.Description.ToLower().Contains(searchTerm.ToLower())))
                .Select(i => new
                {
                    id = i.Id, // this needs to be id (not Id) so the select2 box maps it correctly to the <option> id value.
                        InvoiceNumber = i.InvoiceNumber,
                    InvoiceDate = i.InvoiceDate,
                    ServiceRequest = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue) == null ? null : new
                    {
                        Id = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequest.Id,
                        ClaimantName = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequest.ClaimantName,
                        ServiceCode = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequest.Service.Code,
                        City = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequest.Address.City_CityId.Name,
                        ProvinceCode = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequest.Address.Province.ProvinceCode
                    }
                })
                .ToList();

            return Json(new
            {
                total_count = data.Count(),
                items = data
            }, JsonRequestBehavior.AllowGet);
        }

        #region InvoiceAPI

        [HttpPost]
        [AuthorizeRole(Feature = Accounting.CreateInvoice)]
        public ActionResult Create(int serviceRequestId)
        {
            var serviceRequest =
                db.ServiceRequests
                    .WithId(serviceRequestId)
                    .Select(ServiceRequestDto.FromServiceRequestEntityForInvoice.Expand())
                    .First();

            // get the price
            var serviceCatalogues = db.GetServiceCatalogueForCompany(serviceRequest.Physician.Id, serviceRequest.Company.Id).ToList();
            var query = serviceCatalogues.Where(c => c.ServiceId == serviceRequest.Service.Id);
            if (serviceRequest.HasAppointment)
            {
                query = query.Where(sc => sc.LocationId == serviceRequest.Address.CityId);
            }
            else
            {
                query = query.Where(sc => sc.LocationId == 0);
            }
            var serviceCatalogue = query.SingleOrDefault();

            // get the rates
            var rates = db.GetServiceCatalogueRate(serviceRequest.Physician.Id, serviceRequest.CompanyGuid).FirstOrDefault();
            var noShowRate = rates == null ? 1 : rates.NoShowRate.GetValueOrDefault(1);
            var lateCancellationRate = rates == null ? 1 : rates.LateCancellationRate.GetValueOrDefault(1);

            var serviceProvider = db.BillableEntities.First(c => c.EntityGuid == serviceRequest.Physician.Id);
            var customer = db.BillableEntities.First(c => c.EntityGuid == serviceRequest.CompanyGuid);

            // REFACTOR: this is dupped in CreateInvoice
            // Gets new invoice number specific to the service provider except for Shariff, Zeeshan and Rajiv.
            var invoiceNumber = db.Invoices.GetNextInvoiceNumber(serviceProvider.EntityGuid.Value);
            if (serviceProvider.EntityId == "8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c" || serviceProvider.EntityId == "8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9" || serviceProvider.EntityId == "48f9d9fd-deb5-471f-9454-066430a510f1") // Shariff, Zeeshan, Rajiv will use old invoice number approach
            {
                var invoiceNumberStr = db.GetNextInvoiceNumber().First().NextInvoiceNumber;
                invoiceNumber = long.Parse(invoiceNumberStr);
            }

            var invoiceDate = now;
            if (serviceRequest.HasAppointment)
            {
                invoiceDate = serviceRequest.AppointmentDate.Value;
            }

            var invoice = new Orvosi.Data.Invoice();
            invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, string.Empty, identity.Name);

            var invoiceDetail = new Orvosi.Data.InvoiceDetail();
            invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, loggedInUserId.ToString(), serviceCatalogue.Price.GetValueOrDefault(0), rates.NoShowRate.GetValueOrDefault(1), rates.LateCancellationRate.GetValueOrDefault(1));
            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.CalculateTotal();

            db.Invoices.Add(invoice);

            db.SaveChanges();

            return Json(new
            {
                id = invoice.Id
            });
        }

        [AuthorizeRole(Feature = Accounting.CreateInvoice)]
        public ActionResult CreateInvoice(Guid? serviceProviderId)
        {
            // TODO: Need to filter this list to show customers specific to the service provider.
            ViewBag.CustomerSelectList = GetCustomerList();

            var invoice = new Orvosi.Data.Invoice()
            {
                ServiceProviderGuid = physicianOrLoggedInUserId,
                InvoiceDate = now
            };
            return View(invoice);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Accounting.CreateInvoice)]
        public ActionResult CreateInvoice(CreateInvoiceForm form)
        {
            var serviceProvider = db.BillableEntities.First(c => c.EntityGuid == form.ServiceProviderGuid);
            var customer = db.BillableEntities.First(c => c.EntityGuid == form.CustomerGuid);

            // REFACTOR: this is dupped in Create
            // Gets new invoice number specific to the service provider except for Shariff, Zeeshan and Rajiv.
            var invoiceNumber = db.Invoices.GetNextInvoiceNumber(serviceProvider.EntityGuid.Value);
            if (serviceProvider.EntityId == "8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c" || serviceProvider.EntityId == "8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9" || serviceProvider.EntityId == "48f9d9fd-deb5-471f-9454-066430a510f1") // Shariff, Zeeshan, Rajiv will use old invoice number approach
            {
                // this is the old stored procedure approach. This same code is in the Mapper class if ever refactored.
                var invoiceNumberStr = db.GetNextInvoiceNumber().First().NextInvoiceNumber;
                invoiceNumber = long.Parse(invoiceNumberStr);
            }

            var invoiceDate = form.InvoiceDate;

            var invoice = new Orvosi.Data.Invoice();
            invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, string.Empty, identity.Name);

            db.Invoices.Add(invoice);
            db.SaveChanges();
            return RedirectToAction("AllInvoices", new FilterArgs() { ServiceProviderId = form.ServiceProviderGuid, InvoiceId = invoice.Id });
        }

        [AuthorizeRole(Feature = Accounting.ViewInvoice)]
        public async Task<ActionResult> PreviewInvoice(int id)
        {
            var invoice = await db.Invoices.Include(i => i.InvoiceDetails).FirstAsync(c => c.Id == id);
            return PartialView("~/Views/Invoice/PrintableInvoice.cshtml", invoice);
        }

        [AuthorizeRole(Feature = Accounting.ViewInvoice)]
        public async Task<ActionResult> DownloadInvoice(int id)
        {
            var invoice = await db.Invoices.FindAsync(id);
            var invoiceFolder = await db.Physicians.FindAsync(invoice.ServiceProviderGuid);

            var box = new BoxManager();
            var boxFile = await box.GetInvoiceFileInfo(invoice.BoxFileId, invoiceFolder.BoxInvoicesFolderId, invoice.ObjectGuid);
            if (boxFile == null)
            {
                var content = HtmlHelpers.RenderViewToString(this.ControllerContext, "~/Views/Invoice/PrintableInvoice.cshtml", invoice);
                invoice = await accountingService.GenerateInvoicePdf(invoice, content);
            }
            boxFile = await box.GetInvoiceFileInfo(invoice.BoxFileId, invoiceFolder.BoxInvoicesFolderId, invoice.ObjectGuid);
            var fileStream = await box.GetFileStream(invoice.BoxFileId);

            // Make the file a downloadable attachment - comment this out to show it directly inside
            HttpContext.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", boxFile.Name));

            // Return the file as a PDF
            return new FileStreamResult(fileStream, "application/pdf");
        }

        [Route("Accounting/Download/{id}")]
        [Route("Invoice/Download/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Download(Guid id)
        {
            var invoice = await db.Invoices.FirstOrDefaultAsync(i => i.ObjectGuid == id);
            var invoiceFolder = await db.Physicians.FindAsync(invoice.ServiceProviderGuid);

            var box = new BoxManager();
            var boxFile = await box.GetInvoiceFileInfo(invoice.BoxFileId, invoiceFolder.BoxInvoicesFolderId, invoice.ObjectGuid);
            if (boxFile == null)
            {
                var content = HtmlHelpers.RenderViewToString(this.ControllerContext, "~/Views/Invoice/PrintableInvoice.cshtml", invoice);
                invoice = await accountingService.GenerateInvoicePdf(invoice, content);
            }
            boxFile = await box.GetInvoiceFileInfo(invoice.BoxFileId, invoiceFolder.BoxInvoicesFolderId, invoice.ObjectGuid);
            var fileStream = await box.GetFileStream(invoice.BoxFileId);

            // Make the file a downloadable attachment - comment this out to show it directly inside
            HttpContext.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", boxFile.Name));

            // Return the file as a PDF
            return new FileStreamResult(fileStream, "application/pdf");
        }

        [AuthorizeRole(Feature = Accounting.ViewInvoice)]
        public async Task<ActionResult> GenerateInvoicePdf(int invoiceId)
        {
            var invoice = await db.Invoices.FindAsync(invoiceId);
            var content = HtmlHelpers.RenderViewToString(this.ControllerContext, "~/Views/Invoice/PrintableInvoice.cshtml", invoice);
            invoice = await accountingService.GenerateInvoicePdf(invoice, content);
            
            // Return the file as a PDF
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.SendInvoice)]
        public async Task<ActionResult> EditAndSendInvoice(int invoiceId, int? serviceRequestId)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.Id == invoiceId);

            var message = BuildSendInvoiceMailMessage(invoice, Request.GetBaseUrl());
            var viewModel = new WebApp.ViewModels.MailMessageViewModel
            {
                InvoiceId = invoiceId,
                ServiceRequestId = serviceRequestId,
                Message = message
            };
            return PartialView("_EditAndSendInvoice", viewModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Accounting.SendInvoice)]
        public async Task<ActionResult> EditAndSendInvoice()
        {
            var message = new MailMessage();
            message.From = new MailAddress(Request.Form["Message.From"]);
            message.To.Add(Request.Form["Message.To"]);

            var cc = Request.Form["Message.CC"];
            if (!string.IsNullOrEmpty(cc))
                message.CC.Add(cc);

            message.Subject = Request.Form["Message.Subject"];
            message.Body = Request.Form["Message.Body"];

            var invoiceId = int.Parse(Request.Form["InvoiceId"]);
            var invoice = await db.Invoices.FindAsync(invoiceId);
            var physician = await db.Physicians.FindAsync(invoice.ServiceProviderGuid);

            if (Request.Form["IncludeAttachment"] == "on")
            {
                var box = new BoxManager();
                var file = await box.GetInvoiceFileInfo(invoice.BoxFileId, physician.BoxInvoicesFolderId, invoice.ObjectGuid);
                if (file == null)
                {
                    var content = HtmlHelpers.RenderViewToString(this.ControllerContext, "~/Views/Invoice/PrintableInvoice.cshtml", invoice);
                    invoice = await accountingService.GenerateInvoicePdf(invoice, content);
                    file = await box.GetInvoiceFileInfo(invoice.BoxFileId, physician.BoxInvoicesFolderId, invoice.ObjectGuid);
                }
                var fileStream = await box.GetFileStream(file.Id);
                var invoiceAttachment = new Attachment(fileStream, file.Name);
                message.Attachments.Add(invoiceAttachment);
            }

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file.ContentLength != 0)
                {
                    var attachment = new Attachment(file.InputStream, file.FileName);
                    message.Attachments.Add(attachment);
                }
            }

            // send the email
            invoice = await SendMessageUsingGoogle(invoice, message);

            // set the submit invoice task to done
            foreach (var item in invoice.InvoiceDetails.Where(id => id.ServiceRequestId.HasValue))
            {
                var task = item.ServiceRequest.ServiceRequestTasks.FirstOrDefault(c => c.TaskId == Tasks.SubmitInvoice); // TODO: 37 is submit invoice for add/pr
                await workService.ChangeTaskStatus(task.Id, TaskStatuses.Done);
            }

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.SendInvoice)]
        public async Task<ActionResult> SendInvoice(int invoiceId)
        {
            var invoice = await db.Invoices.FirstAsync(c => c.Id == invoiceId);

            // send the email
            var message = BuildSendInvoiceMailMessage(invoice, Request.GetBaseUrl());
            invoice = await SendMessageUsingGoogle(invoice, message);

            // set the submit invoice task to done
            foreach (var item in invoice.InvoiceDetails.Where(id => id.ServiceRequestId.HasValue))
            {
                var task = item.ServiceRequest.ServiceRequestTasks.FirstOrDefault(c => c.TaskId == Tasks.SubmitInvoice); // TODO: 37 is submit invoice for add/pr
                await workService.ChangeTaskStatus(task.Id, TaskStatuses.Done);
            }

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private async Task<Invoice> SendMessageUsingGoogle(Invoice invoice, MailMessage message)
        {
            // this should get created using a DI container and configured in the Startup.
            await new GoogleServices()
                .SendEmailAsync(message);

            invoice.SentDate = now;
            invoice.ModifiedDate = now;
            invoice.ModifiedUser = identity.Name;

            invoice.InvoiceSentLogs.Add(new Orvosi.Data.InvoiceSentLog()
            {
                InvoiceId = invoice.Id,
                EmailTo = invoice.CustomerEmail,
                SentDate = now,
                ModifiedDate = now,
                ModifiedUser = identity.Name
            });

            return invoice;
        }

        [AuthorizeRole(Feature = Accounting.EditInvoice)]
        public ActionResult EditInvoiceHeader(int invoiceId)
        {
            var model = db.Invoices
                    .WithId(invoiceId)
                    .Select(InvoiceProjections.Header())
                    .First();

            return PartialView("_EditInvoiceHeader", model);
            // var editForm = new Models.AccountingModel.Mapper(context).MapToEditForm(invoiceDetailId);

            //return Json(editForm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Accounting.EditInvoice)]
        public ActionResult EditInvoiceHeader(Orvosi.Shared.Model.Invoice invoice)
        {
            var record = db.Invoices.Find(invoice.Id);
            record.InvoiceDate = invoice.InvoiceDate;
            record.Terms = invoice.Terms;
            record.TaxRateHst = invoice.TaxRateHst;
            record.CustomerEmail = invoice.Customer.BillingEmail;
            record.ModifiedDate = now;
            record.ModifiedUser = loggedInUserId.ToString();
            record.CalculateTotal();
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.EditInvoice)]
        public ActionResult EditInvoiceItem(int invoiceDetailId)
        {
            var editForm = db.InvoiceDetails
                    .Where(id => id.Id == invoiceDetailId)
                    .Select(InvoiceProjections.EditItemForm())
                    .Single();

            return PartialView("_EditInvoiceItem", editForm);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Accounting.EditInvoice)]
        public ActionResult EditInvoiceItem(Orvosi.Shared.Model.InvoiceDetail invoiceDetail)
        {
            var record = db.InvoiceDetails.Find(invoiceDetail.Id);
            var invoiceDto = InvoiceDto.FromInvoiceEntity.Invoke(record.Invoice);
            if (invoiceDto.IsSent)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot change an invoice that has already been sent.");
            }
            record.Description = invoiceDetail.Description;
            record.AdditionalNotes = invoiceDetail.AdditionalNotes;
            record.Amount = invoiceDetail.Amount;
            record.Rate = invoiceDetail.Rate;
            record.ModifiedDate = now;
            record.ModifiedUser = loggedInUserId.ToString();

            // invoice detail passed in will not have the service request information
            if (record.ServiceRequestId.HasValue && invoiceDetail.Rate != 1)
            {
                var discountType = record.ServiceRequest.GetDiscountType();
                record.DiscountDescription = InvoiceHelper.GetDiscountDescription(discountType, invoiceDetail.Rate, invoiceDetail.Amount);
            }
            else
            {
                record.DiscountDescription = null;
            }

            record.CalculateTotal();
            db.SaveChanges();

            var invoice = db.Invoices.Find(invoiceDetail.Invoice.Id);
            invoice.CalculateTotal();

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.EditInvoice)]
        public ActionResult AddInvoiceItem(int invoiceId)
        {

            var newItem = new Orvosi.Data.InvoiceDetail()
            {
                Description = "New Item",
                InvoiceId = invoiceId,
                Amount = 0,
                Rate = 1
            };
            newItem.CalculateTotal();
            db.InvoiceDetails.Add(newItem);
            db.SaveChanges();
            db.Entry(newItem).Reload();

            return Json(new
            {
                data = newItem
            });
            // var editForm = new Models.AccountingModel.Mapper(context).MapToEditForm(invoiceDetailId);

            //return Json(editForm, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(Feature = Accounting.EditInvoice)]
        public ActionResult DeleteInvoiceItem(int invoiceDetailId)
        {
            var item = db.InvoiceDetails.Find(invoiceDetailId);

            var invoiceDto = InvoiceDto.FromInvoiceEntity.Invoke(item.Invoice);
            if (invoiceDto.IsSent)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot change an invoice that has already been sent.");
            }

            db.InvoiceDetails.Remove(item);
            item.CalculateTotal();
            db.SaveChanges();

            var invoice = db.Invoices.Find(item.InvoiceId);
            invoice.CalculateTotal();
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.DeleteInvoice)]
        public ActionResult DeleteInvoice(int invoiceId)
        {
            var invoice = db.Invoices.Find(invoiceId);

            var invoiceDto = InvoiceDto.FromInvoiceEntity.Invoke(invoice);
            if (invoiceDto.IsPaid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot delete an invoice that has already been paid.");
            }

            invoice.IsDeleted = true;
            invoice.DeletedBy = loggedInUserId;
            invoice.DeletedDate = now;

            invoice.InvoiceDetails.ForEach(id =>
            {
                id.IsDeleted = true;
                id.DeletedBy = loggedInUserId;
                id.DeletedDate = now;
            });

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.DeleteInvoice)]
        public ActionResult UndeleteInvoice(int invoiceId)
        {

            var invoice = db.Invoices.Find(invoiceId);

            var invoiceDto = InvoiceDto.FromInvoiceEntity.Invoke(invoice);
            if (invoiceDto.IsPaid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot delete an invoice that has already been paid.");
            }

            invoice.IsDeleted = false;
            invoice.DeletedBy = null;
            invoice.DeletedDate = null;

            invoice.InvoiceDetails.ForEach(id =>
            {
                id.IsDeleted = false;
                id.DeletedBy = null;
                id.DeletedDate = null;
            });

            db.SaveChanges();

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

            ViewData["BaseUrl"] = baseUrl; //This is needed because the full address needs to be included in the email download link
            message.Body = HtmlHelpers.RenderPartialViewToString(this, "~/Views/Shared/NotificationTemplates/InvoiceDownloadLink.cshtml", invoice);

            return message;
        }

        #endregion

        #region ReceiptAPI

        [AuthorizeRole(Feature = Accounting.ManageReceipts)]
        public ActionResult CreateReceipt(int invoiceId, decimal? amount)
        {
            var invoice = db.Invoices.WithId(invoiceId).First();
            var id = Guid.NewGuid();
            var receipt = new Receipt()
            {
                Id = id,
                InvoiceId = invoiceId,
                ReceivedDate = SystemTime.UtcNow(),
                // when the amount is null the invoice is paid in full (it is ignored either way)
                Amount = !amount.HasValue ? invoice.Total.Value : amount.Value,
                CreatedDate = now,
                CreatedUser = loggedInUserId.ToString(),
                ModifiedDate = now,
                ModifiedUser = loggedInUserId.ToString()
            };
            db.Receipts.Add(receipt);
            db.SaveChanges();

            return Json(new
            {
                id = receipt.Id
            });
        }

        [AuthorizeRole(Feature = Accounting.ManageReceipts)]
        public ActionResult DeleteReceipt(Guid receiptId)
        {
            var receipt = db.Receipts.WithId(receiptId).First();
            db.Receipts.Remove(receipt);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [AuthorizeRole(Feature = Accounting.ManageReceipts)]
        public ActionResult EditReceipt(Receipt receipt)
        {
            var record = db.Receipts.WithId(receipt.Id).First();
            record.ReceivedDate = receipt.ReceivedDate;
            record.InvoiceId = receipt.InvoiceId;
            if (receipt.IsPaidInFull)
            {
                var invoice = db.Invoices.WithId(receipt.InvoiceId).First();
                record.Amount = record.IsPaidInFull ? invoice.Total.Value : receipt.Amount;
            }
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        private List<SelectListItem> GetCustomerList()
        {
            return db.Companies
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.ObjectGuid.ToString(),
                    Group = new SelectListGroup() { Name = c.Parent.Name }
                }).ToList();
        }

    }
}