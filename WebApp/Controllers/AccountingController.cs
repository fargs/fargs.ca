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
using System.Linq.Expressions;
using Orvosi.Data;
using Orvosi.Data.Filters;
using WebApp.Library.Projections;
using MoreLinq;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Case Coordinator, Super Admin, Physician")]
    public class AccountingController : Controller
    {
        public ActionResult UnsentInvoices(Guid? serviceProviderId)
        {
            using (var context = new OrvosiDbContext())
            {
                Guid userId = GetServiceProviderId(serviceProviderId);
                var now = SystemTime.Now();

                var serviceRequests = context.ServiceRequests
                    .ForPhysician(userId)
                    .AreNotCancellations()
                    .HaveNotCompletedSubmitInvoiceTask()
                    .Select(ServiceRequestProjections.BasicInfo(userId, now))
                    .ToList();

                var invoices = context.Invoices
                    .AreOwnedBy(userId)
                    .AreNotDeleted()
                    .AreNotSent()
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
                            .ThenBy(sr => sr.ServiceRequest.StartTime)
                    }).OrderBy(df => df.Day);

                ViewBag.SelectedUserId = userId;

                return View("Invoices", model);
            }
        }

        public ActionResult SentInvoices(Guid? serviceProviderId)
        {
            using (var context = new OrvosiDbContext())
            {
                Guid userId = GetServiceProviderId(serviceProviderId);
                var now = SystemTime.Now();

                var serviceRequests = context.ServiceRequests
                    .ForPhysician(userId)
                    .HaveCompletedSubmitInvoiceTask()
                    .Select(ServiceRequestProjections.BasicInfo(userId, now))
                    .ToList();

                var invoices = context.Invoices
                    .AreOwnedBy(userId)
                    .AreSent()
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
                            .ThenBy(sr => sr.ServiceRequest.StartTime)
                    }).OrderBy(df => df.Day);


                ViewBag.SelectedUserId = userId;

                return View("Invoices", model);
            }
        }

        public ActionResult AllInvoices(Guid? serviceProviderId)
        {
            Guid userId = GetServiceProviderId(serviceProviderId);

            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var model = context
                    .Invoices
                    .AreOwnedBy(userId)
                    .Select(InvoiceProjections.InvoiceList(userId, System.SystemTime.Now()))
                    .ToList();
                ViewBag.SelectedUserId = userId;

                return View("AllInvoices", model);
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
                    .Single();

                return PartialView("_Invoice", record);
            }
        }

        // API

        [HttpPost]
        public ActionResult Create(int serviceRequestId)
        {
            var repo = new Mapper(new Orvosi.Data.OrvosiDbContext());
            repo.Create(serviceRequestId, User);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult AddInvoice(Guid? serviceProviderId)
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