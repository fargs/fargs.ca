// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

namespace Orvosi.Data
{

    // Invoice
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class Invoice
    {
        public int Id { get; set; } // Id (Primary key)
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public string InvoiceNumber { get; set; } // InvoiceNumber (length: 128)
        public System.DateTime InvoiceDate { get; set; } // InvoiceDate
        public string Currency { get; set; } // Currency (length: 128)
        public string Terms { get; set; } // Terms (length: 128)
        public System.DateTime? DueDate { get; set; } // DueDate
        public System.Guid ServiceProviderGuid { get; set; } // ServiceProviderGuid
        public string ServiceProviderName { get; set; } // ServiceProviderName (length: 128)
        public string ServiceProviderEntityType { get; set; } // ServiceProviderEntityType (length: 50)
        public string ServiceProviderLogoCssClass { get; set; } // ServiceProviderLogoCssClass (length: 128)
        public string ServiceProviderEmail { get; set; } // ServiceProviderEmail (length: 128)
        public string ServiceProviderPhoneNumber { get; set; } // ServiceProviderPhoneNumber (length: 128)
        public string ServiceProviderAddress1 { get; set; } // ServiceProviderAddress1 (length: 128)
        public string ServiceProviderAddress2 { get; set; } // ServiceProviderAddress2 (length: 128)
        public string ServiceProviderCity { get; set; } // ServiceProviderCity (length: 128)
        public string ServiceProviderPostalCode { get; set; } // ServiceProviderPostalCode (length: 128)
        public string ServiceProviderProvince { get; set; } // ServiceProviderProvince (length: 128)
        public string ServiceProviderCountry { get; set; } // ServiceProviderCountry (length: 128)
        public System.Guid CustomerGuid { get; set; } // CustomerGuid
        public string CustomerName { get; set; } // CustomerName (length: 128)
        public string CustomerEntityType { get; set; } // CustomerEntityType (length: 50)
        public string CustomerAddress1 { get; set; } // CustomerAddress1 (length: 128)
        public string CustomerAddress2 { get; set; } // CustomerAddress2 (length: 128)
        public string CustomerCity { get; set; } // CustomerCity (length: 128)
        public string CustomerPostalCode { get; set; } // CustomerPostalCode (length: 128)
        public string CustomerProvince { get; set; } // CustomerProvince (length: 128)
        public string CustomerCountry { get; set; } // CustomerCountry (length: 128)
        public string CustomerEmail { get; set; } // CustomerEmail (length: 128)
        public decimal? SubTotal { get; set; } // SubTotal
        public decimal? TaxRateHst { get; set; } // TaxRateHst
        public decimal? Discount { get; set; } // Discount
        public decimal? Total { get; set; } // Total
        public System.DateTime? SentDate { get; set; } // SentDate
        public System.DateTime? DownloadDate { get; set; } // DownloadDate
        public System.DateTime? PaymentReceivedDate { get; set; } // PaymentReceivedDate
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public decimal? Hst { get; set; } // Hst
        public bool IsDeleted { get; set; } // IsDeleted
        public System.DateTime? DeletedDate { get; set; } // DeletedDate
        public System.Guid? DeletedBy { get; set; } // DeletedBy
        public string ServiceProviderHstNumber { get; set; } // ServiceProviderHstNumber (length: 50)
        public string BoxFileId { get; set; } // BoxFileId (length: 50)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<InvoiceDetail> InvoiceDetails { get; set; } // InvoiceDetail.FK_InvoiceDetail_Invoice
        public virtual System.Collections.Generic.ICollection<InvoiceSentLog> InvoiceSentLogs { get; set; } // InvoiceSentLog.FK_InvoiceSentLog_Invoice
        public virtual System.Collections.Generic.ICollection<Receipt> Receipts { get; set; } // Receipt.FK_Receipt_Invoice

        public Invoice()
        {
            ObjectGuid = System.Guid.NewGuid();
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            IsDeleted = false;
            InvoiceDetails = new System.Collections.Generic.List<InvoiceDetail>();
            InvoiceSentLogs = new System.Collections.Generic.List<InvoiceSentLog>();
            Receipts = new System.Collections.Generic.List<Receipt>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
