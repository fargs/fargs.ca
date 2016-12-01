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

    // AspNetUsers
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AspNetUserConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserConfiguration()
            : this("dbo")
        {
        }

        public AspNetUserConfiguration(string schema)
        {
            ToTable(schema + ".AspNetUsers");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.Email).HasColumnName(@"Email").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.EmailConfirmed).HasColumnName(@"EmailConfirmed").IsRequired().HasColumnType("bit");
            Property(x => x.PasswordHash).HasColumnName(@"PasswordHash").IsOptional().HasColumnType("nvarchar");
            Property(x => x.SecurityStamp).HasColumnName(@"SecurityStamp").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PhoneNumber).HasColumnName(@"PhoneNumber").IsOptional().HasColumnType("nvarchar");
            Property(x => x.PhoneNumberConfirmed).HasColumnName(@"PhoneNumberConfirmed").IsRequired().HasColumnType("bit");
            Property(x => x.TwoFactorEnabled).HasColumnName(@"TwoFactorEnabled").IsRequired().HasColumnType("bit");
            Property(x => x.LockoutEndDateUtc).HasColumnName(@"LockoutEndDateUtc").IsOptional().HasColumnType("datetime");
            Property(x => x.LockoutEnabled).HasColumnName(@"LockoutEnabled").IsRequired().HasColumnType("bit");
            Property(x => x.AccessFailedCount).HasColumnName(@"AccessFailedCount").IsRequired().HasColumnType("int");
            Property(x => x.UserName).HasColumnName(@"UserName").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.Title).HasColumnName(@"Title").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.FirstName).HasColumnName(@"FirstName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.LastName).HasColumnName(@"LastName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.EmployeeId).HasColumnName(@"EmployeeId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CompanyId).HasColumnName(@"CompanyId").IsOptional().HasColumnType("smallint");
            Property(x => x.CompanyName).HasColumnName(@"CompanyName").IsOptional().HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(256);
            Property(x => x.LastActivationDate).HasColumnName(@"LastActivationDate").IsOptional().HasColumnType("datetime");
            Property(x => x.IsTestRecord).HasColumnName(@"IsTestRecord").IsRequired().HasColumnType("bit");
            Property(x => x.RoleLevelId).HasColumnName(@"RoleLevelId").IsOptional().HasColumnType("tinyint");
            Property(x => x.HourlyRate).HasColumnName(@"HourlyRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.LogoCssClass).HasColumnName(@"LogoCssClass").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ColorCode).HasColumnName(@"ColorCode").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.BoxFolderId).HasColumnName(@"BoxFolderId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.BoxUserId).HasColumnName(@"BoxUserId").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.BoxAccessToken).HasColumnName(@"BoxAccessToken").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.BoxRefreshToken).HasColumnName(@"BoxRefreshToken").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.HstNumber).HasColumnName(@"HstNumber").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Notes).HasColumnName(@"Notes").IsOptional().HasColumnType("nvarchar");

            // Foreign keys
            HasOptional(a => a.Company).WithMany(b => b.AspNetUsers).HasForeignKey(c => c.CompanyId).WillCascadeOnDelete(false); // FK_AspNetUsers_Company
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
