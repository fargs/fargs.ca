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

    // PhysicianLicense
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class PhysicianLicenseConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PhysicianLicense>
    {
        public PhysicianLicenseConfiguration()
            : this("dbo")
        {
        }

        public PhysicianLicenseConfiguration(string schema)
        {
            ToTable(schema + ".PhysicianLicense");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.CollegeName).HasColumnName(@"CollegeName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.LongName).HasColumnName(@"LongName").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.ExpiryDate).HasColumnName(@"ExpiryDate").IsOptional().HasColumnType("date");
            Property(x => x.MemberName).HasColumnName(@"MemberName").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.CertificateClass).HasColumnName(@"CertificateClass").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.IsPrimary).HasColumnName(@"IsPrimary").IsOptional().HasColumnType("bit");
            Property(x => x.Preference).HasColumnName(@"Preference").IsOptional().HasColumnType("tinyint");
            Property(x => x.DocumentId).HasColumnName(@"DocumentId").IsOptional().HasColumnType("smallint");
            Property(x => x.CountryId).HasColumnName(@"CountryId").IsOptional().HasColumnType("smallint");
            Property(x => x.ProvinceId).HasColumnName(@"ProvinceId").IsOptional().HasColumnType("smallint");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>