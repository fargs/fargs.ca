using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Extensions
{
    public static class InvoiceExtensions
    {
        public static string GetFileName(this Invoice invoice)
        {
            return $"Invoice_{invoice.InvoiceNumber}_{invoice.ServiceProviderEmail.Split('@')[0]}_{invoice.CustomerName}_{invoice.ObjectGuid}.pdf";
        }
    }
}
