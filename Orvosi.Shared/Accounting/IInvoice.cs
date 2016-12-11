using System;

namespace Orvosi.Shared.Accounting
{
    public interface IInvoice
    {
        int Id { get; set; }
        string InvoiceNumber { get; set; }
        DateTime InvoiceDate { get; set; }
        DateTime? PaymentDueDate { get; set; }
        decimal? SubTotal { get; set; }
        decimal? TaxRateHst { get; set; }
        decimal? Hst { get; set; }
        decimal? Total { get; set; }
        DateTime? SentDate { get; set; }
        DateTime? PaymentReceivedDate { get; set; }
        ICustomer Customer { get; set; }
        Guid InvoiceGuid { get; set; }
    }
}