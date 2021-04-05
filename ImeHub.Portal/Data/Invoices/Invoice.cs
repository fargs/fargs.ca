using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Invoices
{
    public class Invoice
    {
        public int Id { get; set; }
        public Guid ObjectGuid { get; set; }
        public Guid ServiceProviderGuid { get; set; }
        public string ServiceProviderName { get; set; }
        public Guid CustomerGuid { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }

    }
}
