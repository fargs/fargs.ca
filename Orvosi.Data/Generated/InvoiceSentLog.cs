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

    // InvoiceSentLog
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class InvoiceSentLog
    {
        public int Id { get; set; } // Id (Primary key)
        public int InvoiceId { get; set; } // InvoiceId
        public string EmailTo { get; set; } // EmailTo (length: 128)
        public System.DateTime SentDate { get; set; } // SentDate
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Foreign keys
        public virtual Invoice Invoice { get; set; } // FK_InvoiceSentLog_Invoice

        public InvoiceSentLog()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
