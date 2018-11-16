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

    // PhysicianInvite
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class PhysicianInviteConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<PhysicianInvite>
    {
        public PhysicianInviteConfiguration()
            : this("dbo")
        {
        }

        public PhysicianInviteConfiguration(string schema)
        {
            ToTable(schema + ".PhysicianInvite");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.Email).HasColumnName(@"Email").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.UserId).HasColumnName(@"UserId").IsOptional().HasColumnType("uniqueidentifier");
            Property(x => x.SentDate).HasColumnName(@"SentDate").IsOptional().HasColumnType("datetime");
            Property(x => x.LinkClickedDate).HasColumnName(@"LinkClickedDate").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.PhysicianV2).WithMany(b => b.PhysicianInvites).HasForeignKey(c => c.PhysicianId).WillCascadeOnDelete(false); // FK_PhysicianInvite_Physician
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
