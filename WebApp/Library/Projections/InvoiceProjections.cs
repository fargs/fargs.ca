using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public static class InvoiceProjections
    {
        public static Expression<Func<Orvosi.Data.Invoice, Orvosi.Shared.Model.Invoice>> MinimalInfo()
        {
            return id => new Orvosi.Shared.Model.Invoice
            {
                Id = id.Id,
                SentDate = id.SentDate
            };
        }

        public static Expression<Func<Orvosi.Data.InvoiceDetail, Orvosi.Shared.Model.InvoiceDetail>> EditItemForm()
        {
            return id => new Orvosi.Shared.Model.InvoiceDetail
            {
                Id = id.Id,
                Invoice = new Orvosi.Shared.Model.Invoice
                {
                    Id = id.InvoiceId,
                    InvoiceNumber = id.Invoice.InvoiceNumber
                },
                Description = id.Description,
                Amount = id.Amount.HasValue ? id.Amount.Value : 0,
                Rate = id.Rate.HasValue ? id.Rate.Value : 1,
                AdditionalNotes = id.AdditionalNotes,
                ServiceRequestId = id.ServiceRequestId,
                ServiceRequest = id.ServiceRequest == null ? null : new Orvosi.Shared.Model.ServiceRequest
                {
                    Id = id.ServiceRequest.Id,
                    ClaimantName = id.ServiceRequest.ClaimantName
                }
            };
        }
        public static Expression<Func<Orvosi.Data.Invoice, Orvosi.Shared.Model.Invoice>> Header()
        {
            return i => new Orvosi.Shared.Model.Invoice
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                SubTotal = i.SubTotal,
                TaxRateHst = i.TaxRateHst,
                Hst = i.Hst,
                Total = i.Total.Value,
                SentDate = i.SentDate,
                PaymentReceivedDate = i.PaymentReceivedDate,
                InvoiceGuid = i.ObjectGuid,
                ServiceRequestId = i.InvoiceDetails.FirstOrDefault(id => id.ServiceRequestId.HasValue).ServiceRequestId,
                ServiceProvider = new Orvosi.Shared.Model.ServiceProvider
                {
                    Id = i.ServiceProviderGuid,
                    Name = i.ServiceProviderName,
                    Email = i.ServiceProviderEmail,
                    Province = i.ServiceProviderProvince
                },
                Customer = new Orvosi.Shared.Model.Customer
                {
                    Id = i.CustomerGuid,
                    Name = i.CustomerName,
                    BillingEmail = i.CustomerEmail,
                    City = i.CustomerCity,
                    Province = i.CustomerProvince
                },
                InvoiceDetails = i.InvoiceDetails.Select(id => new Orvosi.Shared.Model.InvoiceDetail
                {
                    Id = id.Id,
                    Description = id.Description,
                    Amount = id.Amount.Value,
                    Rate = id.Rate.HasValue ? id.Rate.Value : 1,
                    AdditionalNotes = id.AdditionalNotes,
                    ServiceRequestId = id.ServiceRequestId
                }),
                InvoiceDetailCount = i.InvoiceDetails.Count(),
                Receipts = i.Receipts.Select(r => new Orvosi.Shared.Model.Receipt
                {
                    Id = r.Id,
                    ReceivedDate = r.ReceivedDate,
                    Amount = r.Amount
                })
            };
        }
        public static Expression<Func<Orvosi.Data.Invoice, Orvosi.Shared.Model.Invoice>> InvoiceList(Guid serviceProviderId, DateTime now)
        {
            return i => new Orvosi.Shared.Model.Invoice
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                SubTotal = i.SubTotal,
                TaxRateHst = i.TaxRateHst,
                Hst = i.Hst,
                Total = i.Total.Value,
                SentDate = i.SentDate,
                PaymentReceivedDate = i.PaymentReceivedDate,
                InvoiceGuid = i.ObjectGuid,
                Customer = new Orvosi.Shared.Model.Customer
                {
                    Id = i.CustomerGuid,
                    Name = i.CustomerName,
                    BillingEmail = i.CustomerEmail,
                    City = i.CustomerCity,
                    Province = i.CustomerProvince
                },
                InvoiceDetails = i.InvoiceDetails.Where(id => !id.IsDeleted && i.ServiceProviderGuid == serviceProviderId).Select(id => new Orvosi.Shared.Model.InvoiceDetail
                {
                    Id = id.Id,
                    Description = id.Description,
                    DiscountDescription = id.DiscountDescription,
                    AdditionalNotes = id.AdditionalNotes,
                    Rate = id.Rate.Value,
                    Amount = id.Amount.Value,
                    Total = id.Total.Value,
                    ServiceRequestId = id.ServiceRequestId,
                    ServiceRequest = !id.ServiceRequestId.HasValue ? null : new Orvosi.Shared.Model.ServiceRequest
                    {
                        Id = id.ServiceRequest.Id,
                        ClaimantName = id.ServiceRequest.ClaimantName,
                        DueDate = id.ServiceRequest.DueDate,
                        AppointmentDate = id.ServiceRequest.AppointmentDate,
                        Now = now,
                        StartTime = id.ServiceRequest.StartTime,
                        CancelledDate = id.ServiceRequest.CancelledDate,
                        IsLateCancellation = id.ServiceRequest.IsLateCancellation,
                        IsNoShow = id.ServiceRequest.IsNoShow,
                        IsClosed = id.ServiceRequest.IsClosed,
                        BoxCaseFolderId = id.ServiceRequest.BoxCaseFolderId,
                        ServiceCataloguePrice = id.ServiceRequest.ServiceCataloguePrice,
                        NoShowRate = id.ServiceRequest.NoShowRate,
                        LateCancellationRate = id.ServiceRequest.LateCancellationRate,
                        Notes = id.ServiceRequest.Notes,
                        PhysicianId = id.ServiceRequest.PhysicianId,
                        ServiceRequestTasks = id.ServiceRequest.ServiceRequestTasks.Where(srt => srt.TaskId == Tasks.SubmitInvoice).Select(srt => new Orvosi.Shared.Model.ServiceRequestTask
                        {
                            Id = srt.Id,
                            ProcessTask = new Orvosi.Shared.Model.ProcessTask
                            {
                                Id = srt.TaskId.Value
                            },
                            IsObsolete = srt.IsObsolete,
                            CompletedDate = srt.CompletedDate
                        }),
                        Service = new Orvosi.Shared.Model.Service
                        {
                            Id = id.ServiceRequest.Service.Id,
                            Name = id.ServiceRequest.Service.Name,
                            Code = id.ServiceRequest.Service.Code
                        },
                        Company = new Orvosi.Shared.Model.Company
                        {
                            Id = id.ServiceRequest.Company.Id,
                            Name = id.ServiceRequest.Company.Name
                        },
                        Address = id.ServiceRequest.Address == null ? null : new Orvosi.Shared.Model.Address
                        {
                            Id = id.ServiceRequest.Address.Id,
                            Name = id.ServiceRequest.Address.Name,
                            City = id.ServiceRequest.Address.City_CityId.Name,
                            ProvinceCode = id.ServiceRequest.Address.Province.ProvinceCode
                        }
                    }
                })
            };
        }
    }
}