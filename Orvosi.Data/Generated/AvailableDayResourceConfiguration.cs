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

    // AvailableDayResource
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class AvailableDayResourceConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<AvailableDayResource>
    {
        public AvailableDayResourceConfiguration()
            : this("dbo")
        {
        }

        public AvailableDayResourceConfiguration(string schema)
        {
            ToTable(schema + ".AvailableDayResource");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.AvailableDayId).HasColumnName(@"AvailableDayId").IsRequired().HasColumnType("smallint");
            Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ResourceRoleId).HasColumnName(@"ResourceRoleId").IsOptional().HasColumnType("uniqueidentifier");

            // Foreign keys
            HasRequired(a => a.AspNetUser).WithMany(b => b.AvailableDayResources).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_UserResource_ToTable
            HasRequired(a => a.AvailableDay).WithMany(b => b.AvailableDayResources).HasForeignKey(c => c.AvailableDayId).WillCascadeOnDelete(false); // FK_AvailableDayResources_ToTable
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
