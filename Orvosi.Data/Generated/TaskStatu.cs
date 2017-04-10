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

    // TaskStatus
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class TaskStatu
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public byte DependentPrecedence { get; set; } // DependentPrecedence
        public byte ServiceRequestPrecedence { get; set; } // ServiceRequestPrecedence
        public string ColorCode { get; set; } // ColorCode (length: 10)

        public TaskStatu()
        {
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
