using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Data.Invoices
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int? ServiceRequestId { get; set; }
        public string Description { get; set; }
        public short? Quantity { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Total { get; set; }
        public decimal? Discount { get; set; } 
        public string DiscountDescription { get; set; }
        public decimal? Amount { get; set; }
        public string AdditionalNotes { get; set; } 
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime? DeletedDate { get; set; }
        public System.Guid? DeletedBy { get; set; }
    }
}
