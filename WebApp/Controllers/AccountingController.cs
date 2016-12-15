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

namespace WebApp.Controllers
{
    [Authorize(Roles = "Super Admin, Physician")]
    public class AccountingController : Controller
    {
        public ActionResult UnsentInvoices(FilterArgs filterArgs)
        {
            using (var context = new OrvosiDbContext())
            {
                Guid userId = GetServiceProviderId(filterArgs.ServiceProviderId);
                var now = SystemTime.Now();

                var serviceRequests = context.ServiceRequests
                    .ForPhysician(userId)
                    .AreNotCancellations()
                    .HaveNotCompletedSubmitInvoiceTask()
                    .AreForCompany(filterArgs.CustomerId)
                    .AreWithinDateRange(SystemTime.Now(), filterArgs.Year, filterArgs.Month)
                    .Select(ServiceRequestProjections.BasicInfo(userId, now))
                    .ToList();

                var invoices = context.Invoices
                    .AreOwnedBy(userId)
                    .AreNotDeleted()
                    .AreNotSent()
                    .AreForCustomer(filterArgs.CustomerId)
                    .AreWithinDateRange(SystemTime.Now(), filterArgs.Year, filterArgs.Month)
                    .Select(InvoiceProjections.Header())
                    .ToList();

                // Full outer join on these 2 lists.
                var leftSide = from sr in serviceRequests
                               join i in invoices on sr.Id equals i.ServiceRequestId into g
                               from sub in g.DefaultIfEmpty()
                               select new Orvosi.Shared.Model.UnsentInvoice
                               {
                                   ServiceRequest = sr,
                                   Invoice = sub
                               };

                var rightSide = from i in invoices
                                where !i.ServiceRequestId.HasValue
                                select new Orvosi.Shared.Model.UnsentInvoice
                                {
                                    Invoice = i
                                };

                var result = leftSide.Concat(rightSide).ToList();

                var model = result
                    .GroupBy(d => new { Day = d.Day })
                    .Select(d => new Orvosi.Shared.Model.UnsentInvoiceDayFolder
                    {
                        DayAndTime = d.Key.Day,
                        UnsentInvoices = d
                            .OrderBy(sr => d.Key.Day)
                            .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                    }).OrderBy(df => df.Day);

                var viewModel = new IndexViewModel
                {
                    FilterArgs = filterArgs,
                    UnsentInvoices = model
                };

                return View("Invoices", viewModel);
            }
        }

        public ActionResult UnpaidInvoices(FilterArgs filterArgs)
        {
            using (var context = new OrvosiDbContext())
            {
                Guid userId = GetServiceProviderId(filterArgs.ServiceProviderId);
                var now = SystemTime.Now();

                var serviceRequests = context.ServiceRequests
                    .ForPhysician(userId)
                    //.HaveCompletedSubmitInvoiceTask()
                    //.AreForCompany(filterArgs.CustomerId)
                    //.AreWithinDateRange(filterArgs.Year, filterArgs.Month)
                    .Select(ServiceRequestProjections.BasicInfo(userId, now))
                    .ToList();

                var invoices = context.Invoices
                    .AreOwnedBy(userId)
                    .AreSent()
                    .AreUnpaid()
                    .AreForCustomer(filterArgs.CustomerId)
                    .AreWithinDateRange(SystemTime.Now(), filterArgs.Year, filterArgs.Month)
                    .Select(InvoiceProjections.Header())
                    .ToList();

                // Full outer join on these 2 lists.
                var leftSide = from i in invoices
                               join sr in serviceRequests on i.ServiceRequestId equals sr.Id into g
                               from sub in g.DefaultIfEmpty()
                               select new Orvosi.Shared.Model.UnsentInvoice
                               {
                                   ServiceRequest = sub,
                                   Invoice = i
                               };

                var model = leftSide
                    .GroupBy(d => new { Day = d.Invoice.InvoiceDate }) // NOTE: Grouped by Invoice Date
                    .Select(d => new Orvosi.Shared.Model.UnsentInvoiceDayFolder
                    {
                        DayAndTime = d.Key.Day,
                        UnsentInvoices = d
                            .OrderBy(sr => d.Key.Day)
                            .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                    }).OrderBy(df => df.Day);

                var viewModel = new IndexViewModel
                {
                    FilterArgs = filterArgs,
                    UnsentInvoices = model
                };

                return View("Invoices", viewModel);
            }
        }

        public ActionResult AllInvoices(FilterArgs filterArgs)
        {
            using (var context = new OrvosiDbContext())
            {
                Guid userId = GetServiceProviderId(filterArgs.ServiceProviderId);
                var now = SystemTime.Now();

                var serviceRequests = context.ServiceRequests
                    .ForPhysician(userId)
                    .Select(ServiceRequestProjections.BasicInfo(userId, now))
                    .ToList();

                var invoices = context.Invoices
                    .AreOwnedBy(userId)
                    .AreNotDeleted()
                    .AreForCustomer(filterArgs.CustomerId)
                    .AreWithinDateRange(SystemTime.Now(), filterArgs.Year, filterArgs.Month)
                    .Select(InvoiceProjections.Header())
                    .ToList();

                // Full outer join on these 2 lists.
                var leftSide = from i in invoices
                               join sr in serviceRequests on i.ServiceRequestId equals sr.Id into g
                               from sub in g.DefaultIfEmpty()
                               select new Orvosi.Shared.Model.UnsentInvoice
                               {
                                   ServiceRequest = sub,
                                   Invoice = i
                               };

                var model = leftSide
                    .GroupBy(d => new { Day = d.Invoice.InvoiceDate }) // NOTE: Grouped by Invoice Date
                    .Select(d => new Orvosi.Shared.Model.UnsentInvoiceDayFolder
                    {
                        DayAndTime = d.Key.Day,
                        UnsentInvoices = d
                            .OrderBy(sr => d.Key.Day)
                            .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                    }).OrderBy(df => df.Day);

                var viewModel = new IndexViewModel
                {
                    FilterArgs = filterArgs,
                    UnsentInvoices = model
                };

                return View("Invoices", viewModel);
            }
        }

        public ActionResult Analytics(FilterArgs filterArgs)
        {
            Guid userId = GetServiceProviderId(filterArgs.ServiceProviderId);
            var now = SystemTime.Now();
            using (var context = new OrvosiDbContext())
            {
                var invoices = context.Invoices
                    .AreOwnedBy(userId)
                    .AreNotDeleted()
                    .AreSent()
                    .AreForCustomer(filterArgs.CustomerId)
                    .AreWithinDateRange(SystemTime.Now(), filterArgs.Year, filterArgs.Month)
                    .Select(InvoiceProjections.Header())
                    .ToList();

                var startDate = filterArgs.Year.HasValue ? new DateTime(filterArgs.Year.Value, 01, 01) : new DateTime(now.Year,1,1);
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
                var billableEntities = context.BillableEntities.Select(be => new { be.EntityGuid, be.EntityName });
                var netIncomeByCompany = invoiceTotals
                    .Join(billableEntities,
                        i => i.CustomerId,
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
                vm.Companies = netIncomeByCompany.Select(c => c.CompanyName).Distinct().ToList();
                vm.NetIncomeByMonth = netIncomeByMonth.Select(c => c.SubTotal).ToList();
                vm.NetIncomeByCompany = netIncomeByCompany.Select(c => c.SubTotal).ToList();
                vm.NetIncome = netIncomeByMonth.Sum(c => c.SubTotal);
                vm.Hst = netIncomeByMonth.Sum(c => c.Hst);
                vm.Expenses = netIncomeByMonth.Sum(c => c.Expenses);
                vm.InvoiceCount = invoiceTotals.Count();
                vm.Invoices = invoices;
                vm.FilterArgs = filterArgs;

                return View("~/Views/Invoice/Dashboard.cshtml", vm);
            }
        }
        
        public ActionResult ServiceRequest(Guid? serviceProviderId, int serviceRequestId)
        {
            using (var context = new OrvosiDbContext())
            {
                Guid userId = GetServiceProviderId(serviceProviderId);
                var now = SystemTime.Now();

                var serviceRequests = context
                    .ServiceRequests
                        .Where(s => s.Id == serviceRequestId)
                        .Select(ServiceRequestProjections.DetailsWithInvoices(userId, now))
                        .ToList();
                if (serviceRequests.Count() != 1)
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);

                return PartialView("_ServiceRequest", serviceRequests.First());
            }
        }

        public ActionResult Invoice(int invoiceId)
        {
            using (var context = new OrvosiDbContext())
            {
                var record = context.Invoices
                    .WithId(invoiceId)
                    .Select(InvoiceProjections.Header())
                    .SingleOrDefault();

                return PartialView("_Invoice", record);
            }
        }

        public ActionResult InvoiceMenu(int serviceRequestId, int invoiceId)
        {
            using (var context = new OrvosiDbContext())
            {
                var serviceRequest = context.ServiceRequests
                    .WithId(serviceRequestId)
                    .Select(ServiceRequestProjections.MinimalInfo())
                    .FirstOrDefault();

                var invoice = context.Invoices
                    .Where(i => i.Id == invoiceId)
                    .Select(InvoiceProjections.MinimalInfo())
                    .FirstOrDefault();

                var unsentInvoice = new Orvosi.Shared.Model.UnsentInvoice
                {
                    ServiceRequest = serviceRequest,
                    Invoice = invoice
                };

                return PartialView("_InvoiceMenu", unsentInvoice);
            }
        }

        #region InvoiceAPI

        [HttpPost]
        public ActionResult Create(int serviceRequestId)
        {
            using (var context = new OrvosiDbContext())
            {
                var serviceRequest =
                    context.ServiceRequests
                        .Find(serviceRequestId);

                // check if the no show rates are set in the request. Migrate old records to use invoices.
                if (!serviceRequest.NoShowRate.HasValue || !serviceRequest.LateCancellationRate.HasValue)
                {
                    var rates = context.GetServiceCatalogueRate(serviceRequest.PhysicianId, serviceRequest.Company.ObjectGuid).First();
                    serviceRequest.NoShowRate = rates.NoShowRate;
                    serviceRequest.LateCancellationRate = rates.LateCancellationRate;
                }

                var serviceProvider = context.BillableEntities.First(c => c.EntityGuid == serviceRequest.PhysicianId);
                var customer = context.BillableEntities.First(c => c.EntityGuid == serviceRequest.Company.ObjectGuid);

                // REFACTOR: this is dupped in CreateInvoice
                // Gets new invoice number specific to the service provider except for Shariff, Zeeshan and Rajiv.
                var invoiceNumber = context.Invoices.GetNextInvoiceNumber(serviceProvider.EntityGuid.Value);
                if (serviceProvider.EntityId == "8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c" || serviceProvider.EntityId == "8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9" || serviceProvider.EntityId == "48f9d9fd-deb5-471f-9454-066430a510f1") // Shariff, Zeeshan, Rajiv will use old invoice number approach
                {
                    var invoiceNumberStr = context.GetNextInvoiceNumber().First().NextInvoiceNumber;
                    invoiceNumber = long.Parse(invoiceNumberStr);
                }

                var invoiceDate = SystemTime.Now();
                if (serviceRequest.Service.ServiceCategoryId == ServiceCategories.IndependentMedicalExam)
                {
                    invoiceDate = serviceRequest.AppointmentDate.Value;
                }

                var invoice = new Orvosi.Data.Invoice();
                invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, string.Empty, User.Identity.Name);

                var invoiceDetail = new Orvosi.Data.InvoiceDetail();
                invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
                invoice.InvoiceDetails.Add(invoiceDetail);

                invoice.CalculateTotal();

                context.Invoices.Add(invoice);

                context.SaveChanges();

                return Json(new
                {
                    id = invoice.Id
                });
            }
        }

        public ActionResult CreateInvoice(Guid? serviceProviderId)
        {
            using (var context = new OrvosiDbContext())
            {
                ViewBag.UserSelectList = GetServiceProviderList(context);
                ViewBag.CustomerSelectList = GetCustomerList(context);
            }

            var invoice = new Orvosi.Data.Invoice()
            {
                ServiceProviderGuid = GetServiceProviderId(serviceProviderId),
                InvoiceDate = SystemTime.Now()
            };
            return View(invoice);
        }

        [HttpPost]
        public ActionResult CreateInvoice(CreateInvoiceForm form)
        {
            using (var context = new OrvosiDbContext())
            {
                var serviceProvider = context.BillableEntities.First(c => c.EntityGuid == form.ServiceProviderGuid);
                var customer = context.BillableEntities.First(c => c.EntityGuid == form.CustomerGuid);

                // REFACTOR: this is dupped in Create
                // Gets new invoice number specific to the service provider except for Shariff, Zeeshan and Rajiv.
                var invoiceNumber = context.Invoices.GetNextInvoiceNumber(serviceProvider.EntityGuid.Value);
                if (serviceProvider.EntityId == "8dd4e180-6e3a-4968-a00d-eeb6d2cc7f0c" || serviceProvider.EntityId == "8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9" || serviceProvider.EntityId == "48f9d9fd-deb5-471f-9454-066430a510f1") // Shariff, Zeeshan, Rajiv will use old invoice number approach
                {
                    // this is the old stored procedure approach. This same code is in the Mapper class if ever refactored.
                    var invoiceNumberStr = context.GetNextInvoiceNumber().First().NextInvoiceNumber;
                    invoiceNumber = long.Parse(invoiceNumberStr);
                }

                var invoiceDate = form.InvoiceDate;

                var invoice = new Orvosi.Data.Invoice();
                invoice.BuildInvoice(serviceProvider, customer, invoiceNumber, invoiceDate, string.Empty, User.Identity.Name);

                context.Invoices.Add(invoice);
                context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public async Task<ActionResult> PreviewInvoice(Guid id)
        {
            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var invoice = await context.Invoices.Include(i => i.InvoiceDetails).FirstAsync(c => c.ObjectGuid == id);
                return PartialView("~/Views/Invoice/PrintableInvoice.cshtml", invoice);
            }
        }

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

        public ActionResult EditInvoiceHeader(int invoiceId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var model = context.Invoices
                    .WithId(invoiceId)
                    .Select(InvoiceProjections.Header())
                    .First();

            return PartialView("_EditInvoiceHeader", model);
            // var editForm = new Models.AccountingModel.Mapper(context).MapToEditForm(invoiceDetailId);

            //return Json(editForm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditInvoiceHeader(Orvosi.Shared.Model.Invoice invoice)
        {
            using (var context = new OrvosiDbContext())
            {
                var record = context.Invoices.Find(invoice.Id);
                record.InvoiceDate = invoice.InvoiceDate;
                record.Terms = invoice.Terms;
                record.TaxRateHst = invoice.TaxRateHst;
                record.CustomerEmail = invoice.Customer.BillingEmail;
                record.ModifiedDate = SystemTime.UtcNow();
                record.ModifiedUser = User.Identity.GetGuidUserId().ToString();
                record.CalculateTotal();
                context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public ActionResult EditInvoiceItem(int invoiceDetailId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var editForm = context.InvoiceDetails
                    .Where(id => id.Id == invoiceDetailId)
                    .Select(InvoiceProjections.EditItemForm())
                    .Single();

            return PartialView("_EditInvoiceItem", editForm);
        }

        [HttpPost]
        public ActionResult EditInvoiceItem(Orvosi.Shared.Model.InvoiceDetail invoiceDetail)
        {
            using (var context = new OrvosiDbContext())
            {
                var record = context.InvoiceDetails.Find(invoiceDetail.Id);
                record.Description = invoiceDetail.Description;
                record.AdditionalNotes = invoiceDetail.AdditionalNotes;
                record.Amount = invoiceDetail.Amount;
                record.Rate = invoiceDetail.Rate;
                record.ModifiedDate = SystemTime.UtcNow();
                record.ModifiedUser = User.Identity.GetGuidUserId().ToString();
                record.CalculateTotal();
                context.SaveChanges();

                var invoice = context.Invoices.Find(invoiceDetail.Invoice.Id);
                invoice.CalculateTotal();

                context.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public ActionResult AddInvoiceItem(int invoiceId)
        {
            using (var context = new OrvosiDbContext())
            {
                var newItem = new Orvosi.Data.InvoiceDetail()
                {
                    Description = "New Item",
                    InvoiceId = invoiceId,
                    Amount = 0,
                    Rate = 1
                };
                context.InvoiceDetails.Add(newItem);
                context.SaveChanges();
                context.Entry(newItem).Reload();

                return Json(new
                {
                    data = newItem
                });
            }
            // var editForm = new Models.AccountingModel.Mapper(context).MapToEditForm(invoiceDetailId);

            //return Json(editForm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteInvoiceItem(int invoiceDetailId)
        {
            using (var context = new OrvosiDbContext())
            {
                var item = context.InvoiceDetails.Find(invoiceDetailId);
                context.InvoiceDetails.Remove(item);
                item.CalculateTotal();
                context.SaveChanges();

                var invoice = context.Invoices.Find(item.InvoiceId);
                invoice.CalculateTotal();
                context.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public ActionResult DeleteInvoice(int invoiceId)
        {
            using (var context = new OrvosiDbContext())
            {
                var item = context.Invoices.Find(invoiceId);
                context.Invoices.Remove(item);
                context.SaveChanges();

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
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

        #endregion

        #region ReceiptAPI

        public ActionResult CreateReceipt(int invoiceId, decimal? amount)
        {
            var userId = User.Identity.GetGuidUserId();
            using (var context = new OrvosiDbContext())
            {
                var invoice = context.Invoices.WithId(invoiceId).First();
                var id = Guid.NewGuid();
                var receipt = new Receipt()
                {
                    Id = id,
                    InvoiceId = invoiceId,
                    ReceivedDate = SystemTime.UtcNow(),
                    // when the amount is null the invoice is paid in full (it is ignored either way)
                    Amount = !amount.HasValue ? invoice.Total.Value : amount.Value,
                    CreatedDate = SystemTime.UtcNow(),
                    CreatedUser = userId.ToString(),
                    ModifiedDate = SystemTime.UtcNow(),
                    ModifiedUser = userId.ToString()
                };
                context.Receipts.Add(receipt);
                context.SaveChanges();

                return Json(new
                {
                    id = receipt.Id
                });
            }
        }

        public ActionResult DeleteReceipt(Guid receiptId)
        {
            using (var context = new OrvosiDbContext())
            {
                var receipt = context.Receipts.WithId(receiptId).First();
                context.Receipts.Remove(receipt);
                context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        public ActionResult EditReceipt(Receipt receipt)
        {
            using (var context = new OrvosiDbContext())
            {
                var record = context.Receipts.WithId(receipt.Id).First();
                record.ReceivedDate = receipt.ReceivedDate;
                record.InvoiceId = receipt.InvoiceId;
                if (receipt.IsPaidInFull)
                {
                    var invoice = context.Invoices.WithId(receipt.InvoiceId).First();
                    record.Amount = record.IsPaidInFull ? invoice.Total.Value : receipt.Amount;
                }
                context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
        }

        #endregion
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

        private List<SelectListItem> GetServiceProviderList(OrvosiDbContext context)
        {
            var userSelectList = (from user in context.AspNetUsers
                                  from userRole in context.AspNetUserRoles
                                  from role in context.AspNetRoles
                                  where user.Id == userRole.UserId && role.Id == userRole.RoleId && userRole.RoleId == AspNetRoles.Physician
                                  select new SelectListItem
                                  {
                                      Text = user.FirstName + " " + user.LastName,
                                      Value = user.Id.ToString(),
                                      Group = new SelectListGroup() { Name = role.Name }
                                  }).ToList();
            return userSelectList;
        }

        private List<SelectListItem> GetCustomerList(OrvosiDbContext context)
        {
            return context.Companies
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.ObjectGuid.ToString(),
                    Group = new SelectListGroup() { Name = c.Parent.Name }
                }).ToList();
        }

    }
}