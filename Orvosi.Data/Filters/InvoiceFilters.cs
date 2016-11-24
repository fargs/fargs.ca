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
    }
}
