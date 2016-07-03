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

    // DocumentTemplate
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class DocumentTemplate
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public System.Guid? OwnedByObjectGuid { get; set; } // OwnedByObjectGuid
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<Document> Documents { get; set; } // Document.FK_Document_DocumentTemplate

        public DocumentTemplate()
        {
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            Documents = new System.Collections.Generic.List<Document>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
