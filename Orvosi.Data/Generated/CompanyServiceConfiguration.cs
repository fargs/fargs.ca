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

    // CompanyService
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class CompanyServiceConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<CompanyService>
    {
        public CompanyServiceConfiguration()
            : this("dbo")
        {
        }

        public CompanyServiceConfiguration(string schema)
        {
            ToTable(schema + ".CompanyService");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.Price).HasColumnName(@"Price").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(250);
            Property(x => x.IsTravelRequired).HasColumnName(@"IsTravelRequired").IsRequired().HasColumnType("bit");

            // Foreign keys
            HasOptional(a => a.ServiceV2).WithMany(b => b.CompanyServices).HasForeignKey(c => c.ServiceId).WillCascadeOnDelete(false); // FK_CompanyService_Service
            HasRequired(a => a.CompanyV2).WithMany(b => b.CompanyServices).HasForeignKey(c => c.CompanyId).WillCascadeOnDelete(false); // FK_CompanyService_Company
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
