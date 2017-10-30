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

    // InvoiceItem
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class InvoiceItem
    {
        public int Id { get; set; } // Id (Primary key)
        public int InvoiceId { get; set; } // InvoiceId
        public int? ServiceRequestId { get; set; } // ServiceRequestId
        public string ServiceRequestDescription { get; set; } // ServiceRequestDescription (length: 1000)
        public string Description { get; set; } // Description (length: 256)
        public decimal? Amount { get; set; } // Amount
        public decimal? Discount { get; set; } // Discount

        ///<summary>
        /// 1 = FLAT, 2 = PERCENTAGE
        ///</summary>
        public byte? DiscountTypeId { get; set; } // DiscountTypeId
        public string DiscountDescription { get; set; } // DiscountDescription (length: 256)
        public decimal? SubTotal { get; set; } // SubTotal
        public decimal? Tax { get; set; } // Tax
        public decimal? TaxRate { get; set; } // TaxRate
        public decimal? Total { get; set; } // Total
        public System.DateTime CreatedDate { get; set; } // CreatedDate
        public string CreatedUser { get; set; } // CreatedUser (length: 100)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public bool IsDeleted { get; set; } // IsDeleted
        public System.DateTime? DeletedDate { get; set; } // DeletedDate
        public System.Guid? DeletedBy { get; set; } // DeletedBy

        // Foreign keys
        public virtual Invoice Invoice { get; set; } // FK_InvoiceItem_Invoice
        public virtual ServiceRequest ServiceRequest { get; set; } // FK_InvoiceItem_ServiceRequest

        public InvoiceItem()
        {
            IsDeleted = false;
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
