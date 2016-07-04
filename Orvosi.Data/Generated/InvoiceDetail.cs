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

    // InvoiceDetail
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class InvoiceDetail
    {
        public int Id { get; set; } // Id (Primary key)
        public int InvoiceId { get; set; } // InvoiceId
        public int? ServiceRequestId { get; set; } // ServiceRequestId
        public string Description { get; set; } // Description (length: 256)
        public short? Quantity { get; set; } // Quantity
        public decimal? Rate { get; set; } // Rate
        public decimal? Total { get; set; } // Total
        public decimal? Discount { get; set; } // Discount
        public string DiscountDescription { get; set; } // DiscountDescription (length: 256)
        public decimal? Amount { get; set; } // Amount
        public string AdditionalNotes { get; set; } // AdditionalNotes (length: 1000)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Foreign keys
        public virtual Invoice Invoice { get; set; } // FK_InvoiceDetail_Invoice
        public virtual ServiceRequest ServiceRequest { get; set; } // FK_InvoiceDetail_ServiceRequest

        public InvoiceDetail()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>