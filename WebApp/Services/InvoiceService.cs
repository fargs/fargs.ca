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
        public Invoice PreviewInvoice(BillableEntity serviceProvider, BillableEntity customer, ServiceRequest serviceRequest)
        {
            var invoice = new Invoice()
            {
                InvoiceNumber = "100001",
                ServiceRequestId = serviceRequest.Id,
                InvoiceDate = SystemTime.Now(),
                DueDate = SystemTime.Now().AddDays(PaymentDueInDays),
                Currency = "CAD",
                CompanyGuid = serviceProvider.EntityGuid,
                CompanyName = serviceProvider.EntityName,
                CompanyLogoCssClass = serviceProvider.LogoCssClass,
                Address1 = serviceProvider.Address1,
                Address2 = string.Format("{0}, {1} {2}", serviceProvider.City, serviceProvider.ProvinceName, serviceProvider.CountryName),
                Address3 = serviceProvider.PostalCode,
                Email = serviceProvider.BillingEmail,
                PhoneNumber = serviceProvider.Phone,
                BillToGuid = customer.EntityGuid,
                BillToName = customer.EntityName,
                BillToAddress1 = customer.Address1,
                BillToAddress2 = string.Format("{0}, {1} {2}", customer.City, customer.ProvinceName, customer.CountryName),
                BillToAddress3 = customer.PostalCode,
                BillToEmail = customer.BillingEmail,
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

        public decimal? GetInvoiceTotal(decimal? subTotal, decimal taxRate)
        {
            return subTotal * (1 + taxRate);
        }
    }
}