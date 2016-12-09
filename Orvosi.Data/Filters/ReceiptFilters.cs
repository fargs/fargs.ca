using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class ReceiptFilters
    {
        public static IQueryable<Receipt> WithId(this IQueryable<Receipt> receipts, Guid id)
        {
            return receipts.Where(i => i.Id == id);
        }
    }
}
