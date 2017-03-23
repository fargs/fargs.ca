using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library
{
    public static class ServiceRequestHelper
    {

        public static void UpdateInvoice(this ServiceRequest serviceRequest, OrvosiDbContext context)
        {
            if (serviceRequest.InvoiceDetails.Count > 0)
            {
                var detail = serviceRequest.InvoiceDetails.First();
                var invoice = detail.Invoice;

                if (serviceRequest.CancelledDate.HasValue && !serviceRequest.IsLateCancellation)
                {
                    context.Invoices.Remove(invoice);
                }
                else
                {
                    var rate =
                            context.ServiceCatalogueRates
                                .FirstOrDefault(sc => sc.ServiceProviderGuid == invoice.ServiceProviderGuid
                                    && sc.CustomerGuid == invoice.CustomerGuid);

                    if (serviceRequest.Company.Parent != null)
                    {
                        rate = rate ?? context.ServiceCatalogueRates
                            .FirstOrDefault(sc => sc.ServiceProviderGuid == invoice.ServiceProviderGuid
                                && serviceRequest.Company.Parent.ObjectGuid == invoice.CustomerGuid);
                    }
                    rate = rate ?? context.ServiceCatalogueRates
                            .First(sc => sc.ServiceProviderGuid == invoice.ServiceProviderGuid);

                    if (serviceRequest.IsNoShow)
                    {
                        detail.ApplyDiscount(DiscountTypes.NoShow, rate.NoShowRate);
                    }
                    else if (serviceRequest.IsLateCancellation)
                    {
                        detail.ApplyDiscount(DiscountTypes.LateCancellation, rate.LateCancellationRate);
                    }
                    else
                    {
                        detail.RemoveDiscount();
                    }
                    detail.Invoice.CalculateTotal();
                }
            }
        }
    }
}