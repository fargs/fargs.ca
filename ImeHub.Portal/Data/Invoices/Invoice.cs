using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Invoices
{
    public class Invoice
    {
        public int Id { get; set; }
        public System.Guid ObjectGuid { get; set; }
        public string InvoiceNumber { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string Currency { get; set; }
        public string Terms { get; set; }
        public System.DateTime? DueDate { get; set; }
        public System.Guid ServiceProviderGuid { get; set; }
        public string ServiceProviderName { get; set; }
        public string ServiceProviderEntityType { get; set; }
        public string ServiceProviderEmail { get; set; }
        public string ServiceProviderPhoneNumber { get; set; }
        public string ServiceProviderAddress1 { get; set; }
        public string ServiceProviderAddress2 { get; set; }
        public string ServiceProviderCity { get; set; }
        public string ServiceProviderPostalCode { get; set; }
        public string ServiceProviderProvince { get; set; }
        public string ServiceProviderCountry { get; set; }
        public System.Guid CustomerGuid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEntityType { get; set; }
        public string CustomerAddress1 { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerPostalCode { get; set; }
        public string CustomerProvince { get; set; } 
        public string CustomerCountry { get; set; } 
        public string CustomerEmail { get; set; } 
        public decimal? SubTotal { get; set; }
        public decimal? TaxRateHst { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Total { get; set; }
        public System.DateTime? SentDate { get; set; }
        public System.DateTime? DownloadDate { get; set; }
        public System.DateTime? PaymentReceivedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public decimal? Hst { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime? DeletedDate { get; set; }
        public System.Guid? DeletedBy { get; set; }
        public string ServiceProviderHstNumber { get; set; }
        public string BoxFileId { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public System.DateTime? DownloadExpiryDate { get; set; }
        public string FileSystemProvider { get; set; }
        public string FileId { get; set; }

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}
