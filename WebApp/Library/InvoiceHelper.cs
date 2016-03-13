﻿using Model;
using Model.Enums;
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
        public const byte PaymentDueInDays = 14;

        public static void BuildInvoice(this Invoice invoice, BillableEntity serviceProvider, BillableEntity customer, string invoiceNumber, DateTime invoiceDate, string userName)
        {
            var TaxRateHst = GetTaxRate(customer.ProvinceName);

            invoice.InvoiceNumber = invoiceNumber;
            invoice.InvoiceDate = invoiceDate;
            invoice.DueDate = SystemTime.Now().AddDays(PaymentDueInDays);
            invoice.Currency = "CAD";
            invoice.ServiceProviderGuid = serviceProvider.EntityGuid;
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
            invoice.CustomerGuid = customer.EntityGuid;
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
            description.AppendLine(string.Format("{0} on {1}", serviceRequest.ClaimantName, serviceRequest.AppointmentDate.Value.ToOrvosiDateFormat()));
            description.AppendLine(serviceRequest.ServiceName);
            description.AppendLine(serviceRequest.City);
            invoiceDetail.Description = description.ToString();
            invoiceDetail.Amount = serviceRequest.EffectivePrice;
            invoiceDetail.Rate = 1;
            invoiceDetail.CalculateTotal();

            if (serviceRequest.IsNoShow)
            {
                if (!serviceRequest.NoShowRate.HasValue)
                {
                    throw new Exception("No show rate must have a value");
                }
                var rate = serviceRequest.NoShowRate;
                invoiceDetail.ApplyDiscount(
                    DiscountTypes.NoShow,
                    rate);
            }
            else if (serviceRequest.IsLateCancellation)
            {
                if (!serviceRequest.LateCancellationRate.HasValue)
                {
                    throw new Exception("Late cancellation rate must have a value");
                }
                var rate = serviceRequest.LateCancellationRate;
                invoiceDetail.ApplyDiscount(
                    DiscountTypes.LateCancellation,
                    rate);
            }
        }

        public static void CalculateTotal(this Invoice invoice)
        {
            if (invoice.InvoiceDetails == null)
            {
                invoice.SubTotal = 0;
            }
            else
            {
                invoice.SubTotal = invoice.InvoiceDetails.Sum(c => c.Total);
            }
            invoice.Total = invoice.SubTotal * (1 + invoice.TaxRateHst);
        }

        public static void CalculateTotal(this InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail.Rate > 1) // this is a flat rate
            {
                invoiceDetail.Total = invoiceDetail.Rate;
            }
            else // this is a percentage
            {
                invoiceDetail.Total = invoiceDetail.Amount * (invoiceDetail.Rate ?? 1);
            }
        }

        public static byte GetDiscountType(this ServiceRequest request)
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
            else if (rate <= 1 && rate > 0)
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
            detail.Rate = null;
            detail.CalculateTotal();
        }

        private static decimal? GetTaxRate(string provinceName)
        {
            switch (provinceName)
            {
                case "British Colombia":
                    return 0.05M;
                default:
                    return 0.13M;
            }
        }
    }
}