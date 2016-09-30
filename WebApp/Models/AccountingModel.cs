using data = Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.AccountingModel
{
    public class Mapper
    {
        private data.IOrvosiDbContext context;
        public Mapper(data.IOrvosiDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DayFolder> MapToToday(Guid serviceProviderId, DateTime now)
        {

            var invoices = (from i in context.Invoices
                                //from id in context.InvoiceDetails
                            where i.IsDeleted == false
                            //group id by new { i.CustomerGuid, i.CustomerName, i.CustomerEmail, i.Id, i.InvoiceNumber, i.InvoiceDate, i.SentDate, id.Description, id.DiscountDescription, id.Rate, id.Amount, id.Total, i.SubTotal, i.Discount, i.Hst, i.TaxRateHst, i.Total, ServiceRequestId = id.ServiceRequestId } into iv
                            select new Invoice
                            {
                                Id = i.Id,
                                InvoiceNumber = i.InvoiceNumber,
                                InvoiceDate = i.InvoiceDate,
                                SubTotal = i.SubTotal,
                                TaxRateHst = i.TaxRateHst,
                                Hst = i.Hst,
                                Total = i.Total.Value,
                                SentDate = i.SentDate,
                                Customer = new Customer
                                {
                                    Id = i.CustomerGuid,
                                    Name = i.CustomerName,
                                    BillingEmail = i.CustomerEmail
                                }
                            });


            var source = context.ServiceRequests
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

            return source // this filters out the days
                .Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) < now.Date)
                .GroupBy(d => new { Day = (d.AppointmentDate.HasValue ? d.AppointmentDate : d.DueDate) })
                .Select(d => new DayFolder
                {
                    Day = d.Key.Day.Value,
                        //Company = d.Key.Company,
                        //Address = d.Key.Address,
                        ServiceRequests = source.Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) == d.Key.Day.Value).OrderBy(sr => (sr.AppointmentDate.HasValue ? sr.AppointmentDate : sr.DueDate)).ThenBy(sr => sr.StartTime)
                }).OrderBy(df => df.Day);
        }
    }
}