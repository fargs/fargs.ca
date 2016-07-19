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

    // AspNetUserRoles
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AspNetUserRoleConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AspNetUserRole>
    {
        public AspNetUserRoleConfiguration()
            : this("dbo")
        {
        }

        public AspNetUserRoleConfiguration(string schema)
        {
            ToTable(schema + ".AspNetUserRoles");
            HasKey(x => new { x.UserId, x.RoleId });

            Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.RoleId).HasColumnName(@"RoleId").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsOptional().HasColumnType("nvarchar").HasMaxLength(256);

            // Foreign keys
            HasRequired(a => a.AspNetRole).WithMany(b => b.AspNetUserRoles).HasForeignKey(c => c.RoleId); // FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId
            HasRequired(a => a.AspNetUser).WithMany(b => b.AspNetUserRoles).HasForeignKey(c => c.UserId); // FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
