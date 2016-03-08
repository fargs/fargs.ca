using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WebApp.Library;

namespace WebApp.Services
{
    public class InvoiceService
    {
        public const byte PaymentDueInDays = 14;
        private OrvosiEntities context;
        public event EventHandler InvoiceDetailsUpdated;

        public InvoiceService(string userName)
        {
            this.context = new OrvosiEntities(userName);
        }

        public Invoice BuildInvoice(string invoiceNumber, BillableEntity serviceProvider, BillableEntity customer, ServiceRequest serviceRequest, string userName)
        {
            var TaxRateHst = GetTaxRate(customer.ProvinceName);

            var invoice = new Invoice()
            {
                InvoiceNumber = invoiceNumber,
                InvoiceDate = serviceRequest.AppointmentDate.Value,
                DueDate = SystemTime.Now().AddDays(PaymentDueInDays),
                Currency = "CAD",
                ServiceProviderGuid = serviceProvider.EntityGuid,
                ServiceProviderName = serviceProvider.EntityName,
                ServiceProviderEntityType = serviceProvider.EntityType,
                ServiceProviderLogoCssClass = serviceProvider.LogoCssClass,
                ServiceProviderAddress1 = serviceProvider.Address1,
                ServiceProviderAddress2 = serviceProvider.Address2,
                ServiceProviderCity = serviceProvider.City,
                ServiceProviderPostalCode = serviceProvider.PostalCode,
                ServiceProviderProvince = serviceProvider.ProvinceName,
                ServiceProviderCountry = serviceProvider.CountryName,
                ServiceProviderEmail = serviceProvider.BillingEmail,
                ServiceProviderPhoneNumber = serviceProvider.Phone,
                CustomerGuid = customer.EntityGuid,
                CustomerName = customer.EntityName,
                CustomerEntityType = customer.EntityType,
                CustomerAddress1 = customer.Address1,
                CustomerAddress2 = customer.Address2,
                CustomerCity = customer.City,
                CustomerPostalCode = customer.PostalCode,
                CustomerProvince = customer.ProvinceName,
                CustomerCountry = customer.CountryName,
                CustomerEmail = customer.BillingEmail,
                TaxRateHst = TaxRateHst,
                ModifiedDate = SystemTime.Now(),
                ModifiedUser = userName
            };

            var invoiceDetail = new InvoiceDetail()
            {
                ServiceRequestId = serviceRequest.Id,
                ModifiedDate = SystemTime.Now(),
                ModifiedUser = userName
            };

            var description = new StringBuilder();
            description.AppendLine(string.Format("{0} on {1}", serviceRequest.ClaimantName, serviceRequest.AppointmentDate.Value.GetDateTimeFormats('d')[0]));
            description.AppendLine(serviceRequest.ServiceName);
            description.AppendLine(serviceRequest.City);
            invoiceDetail.Description = description.ToString();
            invoiceDetail.Amount = serviceRequest.EffectivePrice;
            invoiceDetail.Total = serviceRequest.EffectivePrice;
            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.SubTotal = invoice.InvoiceDetails.Sum(c => c.Total);
            invoice.Total = GetInvoiceTotal(invoice.SubTotal, TaxRateHst);

            return invoice;
        }

        public void ApplyNoShowRate(bool isNoShow, InvoiceDetail detail, decimal? rate)
        {
            if (isNoShow)
            {
                if (rate.HasValue)
                {
                    detail.AdditionalNotes = string.Format("NO SHOW - Rate {0} - Original Amount {1}", rate.HasValue ? rate.Value.ToString("0%") : "NOT SET", detail.Amount);
                    detail.Rate = rate;
                    detail.Total = detail.Amount * detail.Rate;
                }
            }
            else
            {
                detail.AdditionalNotes = null;
                detail.Rate = null;
                detail.Total = detail.Amount;
            }
        }

        public void ApplyLateCancellationRate(bool isLateCancellation, InvoiceDetail detail, decimal? rate)
        {
            if (isLateCancellation)
            {
                if (rate.HasValue)
                {
                    detail.AdditionalNotes = string.Format("LATE CANCELLATION - Rate {0} - Original Amount {1}", rate.HasValue ? rate.Value.ToString("0%") : "NOT SET", detail.Amount);
                    detail.Rate = rate;
                    detail.Total = detail.Amount * detail.Rate;
                }
            }
            else
            {
                detail.AdditionalNotes = null;
                detail.Rate = null;
                detail.Total = detail.Amount;
            }
        }

        private decimal? GetTaxRate(string provinceName)
        {
            switch (provinceName)
            {
                case "British Colombia":
                    return 0.05M;
                default:
                    return 0.13M;
            }
        }

        public decimal? GetInvoiceDetailRate(bool isNoShow, decimal? noShowRate, bool isLateCancellation, decimal? lateCancellationRate)
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

        public decimal? GetInvoiceDetailAmount(decimal? price, decimal? rate)
        {
            return price * rate;
        }

        public decimal? GetInvoiceTotal(decimal? subTotal, decimal? taxRate)
        {
            return subTotal * (1 + taxRate);
        }
    }
}