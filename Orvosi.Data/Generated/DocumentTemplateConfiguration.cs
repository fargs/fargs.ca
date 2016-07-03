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
    public partial class DocumentTemplateConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<DocumentTemplate>
    {
        public DocumentTemplateConfiguration()
            : this("dbo")
        {
        }

        public DocumentTemplateConfiguration(string schema)
        {
            ToTable(schema + ".DocumentTemplate");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.OwnedByObjectGuid).HasColumnName(@"OwnedByObjectGuid").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
