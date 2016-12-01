using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class InvoiceFilters
    {
        public static long GetNextInvoiceNumber(this IQueryable<Invoice> invoices, Guid serviceProviderId)
        {
            var data = invoices
                .Where(i => i.ServiceProviderGuid == serviceProviderId)
                .Select(i => i.InvoiceNumber)
                .ToList();

            return data.Any() ? data.Max(i => long.Parse(i)) + 1 : 1;
        }
        public static IQueryable<Invoice> WithId(this IQueryable<Invoice> invoices, long id)
        {
            return invoices.Where(i => i.Id == id);
        }
        public static IQueryable<Invoice> AreOwnedBy(this IQueryable<Invoice> invoices, Guid userId)
        {
            return invoices.Where(i => i.ServiceProviderGuid == userId);
        }
        public static IQueryable<Invoice> AreSent(this IQueryable<Invoice> invoices)
        {
            return invoices
                .Where(i => i.SentDate.HasValue);
        }
        public static IQueryable<Invoice> AreNotSent(this IQueryable<Invoice> invoices)
        {
            // where there is no invoice yet and the 
            return invoices
                .Where(i => !i.SentDate.HasValue);
        }
        public static IQueryable<Invoice> AreNotDeleted(this IQueryable<Invoice> invoices)
        {
            // where there is no invoice yet and the 
            return invoices
                .Where(i => !i.IsDeleted);
        }

    }
}
