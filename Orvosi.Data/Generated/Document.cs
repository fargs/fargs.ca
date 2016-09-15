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

    // Document
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class Document
    {
        public short Id { get; set; } // Id (Primary key)
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public System.Guid? OwnedByObjectGuid { get; set; } // OwnedByObjectGuid
        public short? DocumentTemplateId { get; set; } // DocumentTemplateId
        public System.DateTime? EffectiveDate { get; set; } // EffectiveDate
        public System.DateTime? ExpiryDate { get; set; } // ExpiryDate
        public string Path { get; set; } // Path (length: 2000)
        public string Extension { get; set; } // Extension (length: 50)
        public string Name { get; set; } // Name (length: 256)
        public byte[] Content { get; set; } // Content
        public string ContentType { get; set; } // ContentType (length: 50)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<PhysicianInsurance> PhysicianInsurances { get; set; } // PhysicianInsurance.FK_PhysicianInsurance_Document
        public virtual System.Collections.Generic.ICollection<PhysicianLicense> PhysicianLicenses { get; set; } // PhysicianLicense.FK_PhysicianLicense_Document

        // Foreign keys
        public virtual DocumentTemplate DocumentTemplate { get; set; } // FK_Document_DocumentTemplate

        public Document()
        {
            ObjectGuid = System.Guid.NewGuid();
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            PhysicianInsurances = new System.Collections.Generic.List<PhysicianInsurance>();
            PhysicianLicenses = new System.Collections.Generic.List<PhysicianLicense>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
