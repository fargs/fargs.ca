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
        public const decimal TaxRateHst = 0.13M;

        public Invoice BuildInvoice(string invoiceNumber, BillableEntity serviceProvider, BillableEntity customer, ServiceRequest serviceRequest, string userName)
        {
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
                ServiceProviderCity = serviceProvider.PostalCode,
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
                CustomerCity = customer.PostalCode,
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
                Rate = GetInvoiceDetailRate(serviceRequest.IsNoShow, serviceRequest.NoShowRate, serviceRequest.IsLateCancellation, serviceRequest.LateCancellationRate),
                ModifiedDate = SystemTime.Now(),
                ModifiedUser = userName
            };

            var description = new StringBuilder();
            description.AppendLine(serviceRequest.ClaimantName);
            description.AppendLine(serviceRequest.ServiceName);
            description.AppendLine(serviceRequest.City);
            if (serviceRequest.IsNoShow)
            {
                description.AppendLine(string.Format("NO SHOW - RATE {0}", invoiceDetail.Rate.Value.ToString("0%")));
            }
            else if (serviceRequest.IsLateCancellation)
            {
                description.AppendLine(string.Format("LATE CANCELLATION - RATE {0}", invoiceDetail.Rate.Value.ToString("0%")));
            }

            invoiceDetail.Description = description.ToString();
            invoiceDetail.Amount = GetInvoiceDetailAmount(serviceRequest.EffectivePrice, invoiceDetail.Rate);
            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.SubTotal = invoiceDetail.Amount;
            invoice.Total = GetInvoiceTotal(invoice.SubTotal, TaxRateHst);

            return invoice;
        }
        public Invoice PreviewInvoice(string invoiceNumber, BillableEntity serviceProvider, BillableEntity customer, ServiceRequest serviceRequest)
        {
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
                ServiceProviderCity = serviceProvider.PostalCode,
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
                CustomerCity = customer.PostalCode,
                CustomerPostalCode = customer.PostalCode,
                CustomerProvince = customer.ProvinceName,
                CustomerCountry = customer.CountryName,
                CustomerEmail = customer.BillingEmail,
                TaxRateHst = TaxRateHst
            };

            var invoiceDetail = new InvoiceDetail()
            {
                ServiceRequestId = serviceRequest.Id,
                Rate = GetInvoiceDetailRate(serviceRequest.IsNoShow, serviceRequest.NoShowRate, serviceRequest.IsLateCancellation, serviceRequest.LateCancellationRate)
            };

            var description = new StringBuilder();
            description.AppendFormat("<dl class='dl-horizontal' style='margin-bottom: 0px;'><dt>Claimant</dt><dd>{0}</dd>", serviceRequest.ClaimantName);
            description.AppendFormat("<dt>Service</dt><dd>{0}</dd>", serviceRequest.ServiceName);
            description.AppendFormat("<dt>Location</dt><dd>{0}, {1}</dd></dl>", serviceRequest.City, serviceRequest.AddressName);
            if (serviceRequest.IsNoShow)
            {
                description.AppendFormat("<p>NO SHOW - RATE {0}</p>", invoiceDetail.Rate.Value.ToString("0%"));
            }
            else if (serviceRequest.IsLateCancellation)
            {
                description.AppendFormat("<p>LATE CANCELLATION - RATE {0}</p>", invoiceDetail.Rate.Value.ToString("0%"));
            }

            invoiceDetail.Description = description.ToString();
            invoiceDetail.Amount = GetInvoiceDetailAmount(serviceRequest.EffectivePrice, invoiceDetail.Rate);

            invoice.InvoiceDetails.Add(invoiceDetail);

            invoice.SubTotal = invoiceDetail.Amount;
            invoice.Total = GetInvoiceTotal(invoice.SubTotal, TaxRateHst);

            return invoice;
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