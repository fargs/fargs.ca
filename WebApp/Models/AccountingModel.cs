using data = Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using WebApp.ViewModels.InvoiceViewModels;
using WebApp.Library.Extensions;
using WebApp.Library;
using System.Security.Principal;
using System.Data.Entity;
using System.Threading.Tasks;
using Orvosi.Data.Filters;

namespace WebApp.Models.AccountingModel
{
    public class Mapper
    {
        private data.IOrvosiDbContext context;
        public Mapper(data.IOrvosiDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DayFolder> MapToServiceRequests(Guid serviceProviderId, DateTime now)
        {
            var source = GetServiceRequests(serviceProviderId, now);

            var filtered = source
                .Where(sr => sr.PhysicianId == serviceProviderId)
                .Where(sr => !sr.IsClosed)
                .Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) <= now.Date) // this filters out the days
                .Where(sr => !sr.InvoiceDetails.Any() || sr.InvoiceDetails.Any(id => !id.Invoice.SentDate.HasValue))
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.ProcessTask.Id == Tasks.SubmitInvoice && !srt.CompletedDate.HasValue && !srt.IsObsolete)) // Where it is not sent or the submit invoices task is not checked)
                .ToList();

            return filtered
                .GroupBy(d => new { Day = (d.AppointmentDate.HasValue ? d.AppointmentDate : d.DueDate) })
                .Select(d => new DayFolder
                {
                    Day = d.Key.Day.Value,
                    //Company = d.Key.Company,
                    //Address = d.Key.Address,
                    ServiceRequests = filtered
                        .Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) == d.Key.Day.Value)
                        .OrderBy(sr => (sr.AppointmentDate.HasValue ? sr.AppointmentDate : sr.DueDate)).ThenBy(sr => sr.StartTime)
                }).OrderBy(df => df.Day);
        }

        public IEnumerable<ServiceRequest> MapToServiceRequest(Guid serviceProviderId, DateTime now, int serviceRequestId)
        {
            var source = GetServiceRequests(serviceProviderId, now);

            // Get a list with one item
            return source.Where(s => s.Id == serviceRequestId).ToList();
        }

        public EditInvoiceDetailForm MapToEditForm(int invoiceDetailId)
        {
            var source = context.InvoiceDetails.Select(id => new
            {
                Id = id.Id,
                CustomerEmail = id.Invoice.CustomerEmail,
                InvoiceDate = id.Invoice.InvoiceDate,
                Amount = id.Amount,
                Rate = id.Rate,
                AdditionalNotes = id.AdditionalNotes,
                ClaimantName = id.ServiceRequest.ClaimantName,
                InvoiceNumber = id.Invoice.InvoiceNumber,
                ServiceRequestId = id.ServiceRequestId.Value
            })
            .First(id => id.Id == invoiceDetailId);

            return new EditInvoiceDetailForm
            {
                Id = source.Id,
                To = source.CustomerEmail,
                InvoiceDate = source.InvoiceDate.ToOrvosiDateFormat(),
                Amount = source.Amount,
                Rate = source.Rate,
                AdditionalNotes = source.AdditionalNotes,
                ClaimantName = source.ClaimantName,
                InvoiceNumber = source.InvoiceNumber,
                ServiceRequestId = source.ServiceRequestId
            };
        }

        public void Create(int serviceRequestId, IPrincipal User)
        {
            var db = context;
            var serviceRequest =
                db.ServiceRequests
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

            var invoice = new data.Invoice();
            invoice.BuildInvoice(serviceProvider, customer, invoiceNumber.NextInvoiceNumber, invoiceDate, User.Identity.Name);
            
            var invoiceDetail = new data.InvoiceDetail();
            invoiceDetail.BuildInvoiceDetailFromServiceRequest(serviceRequest, User.Identity.Name);
            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.CalculateTotal();

            db.Invoices.Add(invoice);
            
            db.SaveChanges();
        }

        private IQueryable<ServiceRequest> GetServiceRequests(Guid serviceProviderId, DateTime now)
        {
            return context.ServiceRequests
                //.Where(d =>
                    //d.ServiceRequestTasks.Any(srt => srt.AssignedTo == serviceProviderId)
                    //d.PhysicianId == serviceProviderId
                    //&& d.InvoiceDetails.Any(id => !id.Invoice.SentDate.HasValue) 
                    //&& d.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && !srt.CompletedDate.HasValue && !srt.IsObsolete) // Where it is not sent or the submit invoices task is not checked
                    //&& !d.IsClosed)
                .Select(sr => new ServiceRequest
                {
                    Id = sr.Id,
                    ClaimantName = sr.ClaimantName,
                    DueDate = sr.DueDate,
                    AppointmentDate = sr.AppointmentDate,
                    Now = now,
                    StartTime = sr.StartTime,
                    CancelledDate = sr.CancelledDate,
                    IsLateCancellation = sr.IsLateCancellation,
                    IsNoShow = sr.IsNoShow,
                    IsClosed = sr.IsClosed,
                    BoxCaseFolderId = sr.BoxCaseFolderId,
                    ServiceCataloguePrice = sr.ServiceCataloguePrice,
                    NoShowRate = sr.NoShowRate,
                    LateCancellationRate = sr.LateCancellationRate,
                    Notes = sr.Notes,
                    PhysicianId = sr.PhysicianId,
                    ServiceRequestTasks = sr.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.SubmitInvoice).Select(srt => new ServiceRequestTask
                    {
                        Id = srt.Id,
                        ProcessTask = new ProcessTask
                        {
                            Id = srt.TaskId.Value
                        },
                        IsObsolete = srt.IsObsolete,
                        CompletedDate = srt.CompletedDate
                    }),
                    Service = new Service
                    {
                        Id = sr.Service.Id,
                        Name = sr.Service.Name,
                        Code = sr.Service.Code
                    },
                    Company = new Company
                    {
                        Id = sr.Company.Id,
                        Name = sr.Company.Name
                    },
                    Address = sr.Address == null ? null : new Address
                    {
                        Id = sr.Address.Id,
                        Name = sr.Address.Name,
                        City = sr.Address.City_CityId.Name,
                        ProvinceCode = sr.Address.Province.ProvinceCode
                    },
                    InvoiceDetails = sr.InvoiceDetails.Where(id => !id.IsDeleted && id.Invoice.ServiceProviderGuid == serviceProviderId).Select(id => new InvoiceDetail
                    {
                        Id = id.Id,
                        Description = id.Description,
                        DiscountDescription = id.DiscountDescription,
                        AdditionalNotes = id.AdditionalNotes,
                        Rate = id.Rate.Value,
                        Amount = id.Amount.Value,
                        Total = id.Total.Value,
                        Invoice = new Invoice
                        {
                            Id = id.Invoice.Id,
                            InvoiceNumber = id.Invoice.InvoiceNumber,
                            InvoiceDate = id.Invoice.InvoiceDate,
                            SubTotal = id.Invoice.SubTotal,
                            TaxRateHst = id.Invoice.TaxRateHst,
                            Hst = id.Invoice.Hst,
                            Total = id.Invoice.Total.Value,
                            SentDate = id.Invoice.SentDate,
                            PaymentReceivedDate = id.Invoice.PaymentReceivedDate,
                            InvoiceGuid = id.Invoice.ObjectGuid,
                            Customer = new Customer
                            {
                                Id = id.Invoice.CustomerGuid,
                                Name = id.Invoice.CustomerName,
                                BillingEmail = id.Invoice.CustomerEmail
                            }
                        }
                    })
                });
        }
    }
}