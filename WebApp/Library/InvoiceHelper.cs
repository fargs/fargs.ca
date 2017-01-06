using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebApp.Library;
using WebApp.Library.Extensions;

namespace WebApp.Library
{
    public static class InvoiceHelper
    {
        public const byte PaymentDueInDays = 60;

        public static void BuildInvoice(this Invoice invoice, BillableEntity serviceProvider, BillableEntity customer, long invoiceNumber, DateTime invoiceDate, string terms, string userName)
        {
            var TaxRateHst = GetTaxRate(customer.ProvinceName);
            var paymentTerms = string.IsNullOrEmpty(terms) ? "90" : terms;
            invoice.InvoiceNumber = invoiceNumber.ToString();
            invoice.InvoiceDate = invoiceDate;
            invoice.Terms = paymentTerms;
            invoice.DueDate = SystemTime.Now().AddDays(int.Parse(paymentTerms));
            invoice.Currency = "CAD";
            invoice.ServiceProviderGuid = serviceProvider.EntityGuid.Value;
            invoice.ServiceProviderName = serviceProvider.EntityName;
            invoice.ServiceProviderEntityType = serviceProvider.EntityType;
            invoice.ServiceProviderLogoCssClass = serviceProvider.LogoCssClass;
            invoice.ServiceProviderAddress1 = serviceProvider.Address1;
            invoice.ServiceProviderAddress2 = serviceProvider.Address2;
            invoice.ServiceProviderCity = serviceProvider.City;
            invoice.ServiceProviderPostalCode = serviceProvider.PostalCode;
            invoice.ServiceProviderProvince = serviceProvider.ProvinceName;
            invoice.ServiceProviderCountry = serviceProvider.CountryName;
            invoice.ServiceProviderEmail = serviceProvider.BillingEmail;
            invoice.ServiceProviderPhoneNumber = serviceProvider.Phone;
            invoice.ServiceProviderHstNumber = serviceProvider.HstNumber;
            invoice.CustomerGuid = customer.EntityGuid.Value;
            invoice.CustomerName = customer.EntityName;
            invoice.CustomerEntityType = customer.EntityType;
            invoice.CustomerAddress1 = customer.Address1;
            invoice.CustomerAddress2 = customer.Address2;
            invoice.CustomerCity = customer.City;
            invoice.CustomerPostalCode = customer.PostalCode;
            invoice.CustomerProvince = customer.ProvinceName;
            invoice.CustomerCountry = customer.CountryName;
            invoice.CustomerEmail = customer.BillingEmail;
            invoice.TaxRateHst = TaxRateHst;
            invoice.ModifiedDate = SystemTime.Now();
            invoice.ModifiedUser = userName;

            invoice.CalculateTotal();
        }

        public static void BuildInvoiceDetailFromServiceRequest(this InvoiceDetail invoiceDetail, ServiceRequest serviceRequest, string userName)
        {
            invoiceDetail.ServiceRequestId = serviceRequest.Id;
            invoiceDetail.ModifiedDate = SystemTime.Now();
            invoiceDetail.ModifiedUser = userName;

            var description = new StringBuilder();
            description.AppendLine(serviceRequest.ClaimantName);
            description.AppendLine(serviceRequest.Service.Name);
            if (serviceRequest.Service.HasAppointment)
            {
                description.AppendFormat("On {0} in {1}", serviceRequest.AppointmentDate.ToOrvosiDateFormat(), serviceRequest.Address.City_CityId.Name);
            }
            invoiceDetail.Description = description.ToString();
            invoiceDetail.Amount = serviceRequest.Price.HasValue ? serviceRequest.Price : serviceRequest.ServiceCataloguePrice.HasValue ? serviceRequest.ServiceCataloguePrice : 0;
            invoiceDetail.Rate = 1;
            invoiceDetail.CalculateTotal();

            if (serviceRequest.IsNoShow)
            {
                var rate = serviceRequest.NoShowRate.HasValue ? serviceRequest.NoShowRate : 1;
                invoiceDetail.ApplyDiscount(
                    DiscountTypes.NoShow,
                    rate);
            }
            else if (serviceRequest.IsLateCancellation)
            {
                var rate = serviceRequest.LateCancellationRate.HasValue ? serviceRequest.LateCancellationRate : 1;
                invoiceDetail.ApplyDiscount(
                    DiscountTypes.LateCancellation,
                    rate);
            }
        }

        public static void CalculateTotal(this Invoice invoice)
        {
            invoice.TaxRateHst = invoice.TaxRateHst.HasValue ? invoice.TaxRateHst.Value : GetTaxRate(invoice.CustomerProvince); // This is to fix issue with Vancouver tax rates set to 13%. Could be taken out once the data is fixed.
            invoice.SubTotal = 0;
            invoice.Hst = 0;
            invoice.Total = 0;
            if (invoice.InvoiceDetails != null)
            {
                invoice.SubTotal = invoice.InvoiceDetails.Sum(c => c.Total);
            }
            invoice.Hst = invoice.SubTotal * invoice.TaxRateHst;
            invoice.Total = invoice.SubTotal + invoice.Hst;
        }

        public static void CalculateTotal(this InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail.Rate > 1) // this is a flat rate
            {
                invoiceDetail.Total = invoiceDetail.Rate;
            }
            else // this is a percentage
            {
                invoiceDetail.Total = invoiceDetail.Amount.GetValueOrDefault(0) * (invoiceDetail.Rate.GetValueOrDefault(1));
            }
        }

        public static byte GetDiscountType(this Orvosi.Shared.Model.ServiceRequest request)
        {
            if (request.IsNoShow)
            {
                return DiscountTypes.NoShow;
            }
            else if (request.IsLateCancellation)
            {
                return DiscountTypes.LateCancellation;
            }
            return 0;
        }

        public static byte GetDiscountType(this Orvosi.Data.ServiceRequest request)
        {
            if (request.IsNoShow)
            {
                return DiscountTypes.NoShow;
            }
            else if (request.IsLateCancellation)
            {
                return DiscountTypes.LateCancellation;
            }
            return 0;
        }

        public static string GetDiscountDescription(byte discountType, decimal? rate, decimal? amount)
        {
            var sb = new StringBuilder();
            if (discountType == DiscountTypes.NoShow)
            {
                sb.Append("NO SHOW - ");
            }
            else if (discountType == DiscountTypes.LateCancellation)
            {
                sb.Append("LATE CANCELLATION - ");
            }

            if (rate > 1)
            {
                sb.Append(string.Format("Flat rate {0}, Original price {1}", rate.HasValue ? rate.Value.ToString("C2") : "NOT SET", amount.Value.ToString("C2")));
            }
            else if (rate < 1 && rate > 0)
            {
                sb.Append(string.Format("{0} of {1}", rate.HasValue ? rate.Value.ToString("0%") : "NOT SET", amount.Value.ToString("C2")));
            }
            return sb.ToString();
        }

        public static void ApplyDiscount(this InvoiceDetail detail, byte discountType, decimal? rate)
        {
            if (!rate.HasValue)
            {
                throw new Exception("Discount rate cannot be null");
            }
            detail.DiscountDescription = GetDiscountDescription(discountType, rate, detail.Amount);
            detail.Rate = rate;
            detail.CalculateTotal();
        }

        public static void RemoveDiscount(this InvoiceDetail detail)
        {
            detail.DiscountDescription = null;
            detail.Rate = 1;
            detail.CalculateTotal();
        }

        private static decimal? GetTaxRate(string provinceName)
        {
            switch (provinceName)
            {
                case "British Columbia":
                    return 0.05M;
                default:
                    return 0.13M;
            }
        }
    }
}