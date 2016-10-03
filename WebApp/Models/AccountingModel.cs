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

            return source // this filters out the days
                .Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) < now.Date
                    && s.ServiceRequestTasks.FirstOrDefault()?.Status.Id == TaskStatuses.ToDo) // this where condition needs to be in the next Select as well.
                .GroupBy(d => new { Day = (d.AppointmentDate.HasValue ? d.AppointmentDate : d.DueDate) })
                .Select(d => new DayFolder
                {
                    Day = d.Key.Day.Value,
                    //Company = d.Key.Company,
                    //Address = d.Key.Address,
                    ServiceRequests = source.Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) == d.Key.Day.Value
                        && s.ServiceRequestTasks.FirstOrDefault()?.Status.Id == TaskStatuses.ToDo)
                        .OrderBy(sr => (sr.AppointmentDate.HasValue ? sr.AppointmentDate : sr.DueDate)).ThenBy(sr => sr.StartTime)
                }).OrderBy(df => df.Day);
        }

        public IEnumerable<ServiceRequest> MapToServiceRequest(Guid serviceProviderId, DateTime now, int serviceRequestId)
        {
            var source = GetServiceRequests(serviceProviderId, now);

            // Get a list with one item
            return source.Where(s => s.Id == serviceRequestId);
        }

        public EditInvoiceDetailForm MapToEditForm(int invoiceDetailId)
        {
            var source = context.InvoiceDetails.Select(id => new
            {
                Id = id.Id,
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
                InvoiceDate = source.InvoiceDate.ToOrvosiDateFormat(),
                Amount = source.Amount,
                Rate = source.Rate,
                AdditionalNotes = source.AdditionalNotes,
                ClaimantName = source.ClaimantName,
                InvoiceNumber = source.InvoiceNumber,
                ServiceRequestId = source.ServiceRequestId
            };
        }

        private List<ServiceRequest> GetServiceRequests(Guid serviceProviderId, DateTime now)
        {
            return context.ServiceRequests
                .Where(d => d.ServiceRequestTasks.Any(srt => srt.AssignedTo == serviceProviderId)
                    && !d.IsClosed)
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
                    ServiceRequestTasks = sr.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.SubmitInvoice).Select(srt => new ServiceRequestTask
                    {
                        Id = srt.Id,
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
                        City = sr.Address.City_CityId.Name
                    },
                    InvoiceDetails = sr.InvoiceDetails.Select(id => new InvoiceDetail
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
                            Customer = new Customer
                            {
                                Id = id.Invoice.CustomerGuid,
                                Name = id.Invoice.CustomerName,
                                BillingEmail = id.Invoice.CustomerEmail
                            }
                        }
                    })
                }).ToList();
        }
    }
}