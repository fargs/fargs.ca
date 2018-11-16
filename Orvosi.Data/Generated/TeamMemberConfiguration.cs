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

    // TeamMember
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class TeamMemberConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<TeamMember>
    {
        public TeamMemberConfiguration()
            : this("dbo")
        {
        }

        public TeamMemberConfiguration(string schema)
        {
            ToTable(schema + ".TeamMember");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            Property(x => x.PhysicianId).HasColumnName(@"PhysicianId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.UserId).HasColumnName(@"UserId").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.RoleId).HasColumnName(@"RoleId").IsRequired().HasColumnType("uniqueidentifier");

            // Foreign keys
            HasRequired(a => a.AspNetRole).WithMany(b => b.TeamMembers).HasForeignKey(c => c.RoleId).WillCascadeOnDelete(false); // FK_TeamMember_Role
            HasRequired(a => a.AspNetUser).WithMany(b => b.TeamMembers).HasForeignKey(c => c.UserId).WillCascadeOnDelete(false); // FK_TeamMember_User
            HasRequired(a => a.PhysicianV2).WithMany(b => b.TeamMembers).HasForeignKey(c => c.PhysicianId).WillCascadeOnDelete(false); // FK_TeamMember_Physician
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
