using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class InvoiceDetailFilters
    {
        public static IQueryable<InvoiceDetail> AreNotDeleted(this IQueryable<InvoiceDetail> invoiceDetails)
        {
            // where there is no invoice yet and the 
            return invoiceDetails
                .Where(i => !i.IsDeleted);
        }
    }
}
