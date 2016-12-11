using System.Collections.Generic;

namespace Orvosi.Shared.Accounting
{
    public interface IInvoiceDetail
    {
        int Id { get; set; }
        string Description { get; set; }
        decimal Rate { get; set; }
        decimal Amount { get; set; }
        decimal Discount { get; set; }
        string DiscountDescription { get; set; }
        decimal Total { get; set; }
        string AdditionalNotes { get; set; }
        IServiceRequest ServiceRequest { get; set; }
    }
}