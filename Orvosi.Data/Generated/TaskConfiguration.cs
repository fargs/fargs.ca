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

    // Task
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class TaskConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Task>
    {
        public TaskConfiguration()
            : this("dbo")
        {
        }

        public TaskConfiguration(string schema)
        {
            ToTable(schema + ".Task");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ObjectGuid).HasColumnName(@"ObjectGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceCategoryId).HasColumnName(@"ServiceCategoryId").IsOptional().HasColumnType("smallint");
            Property(x => x.ServiceId).HasColumnName(@"ServiceId").IsOptional().HasColumnType("smallint");
            Property(x => x.Name).HasColumnName(@"Name").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Guidance).HasColumnName(@"Guidance").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.TaskPhaseId).HasColumnName(@"TaskPhaseId").IsOptional().HasColumnType("tinyint");
            Property(x => x.ResponsibleRoleId).HasColumnName(@"ResponsibleRoleId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.IsBillable).HasColumnName(@"IsBillable").IsOptional().HasColumnType("bit");
            Property(x => x.HourlyRate).HasColumnName(@"HourlyRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.EstimatedHours).HasColumnName(@"EstimatedHours").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.Sequence).HasColumnName(@"Sequence").IsOptional().HasColumnType("smallint");
            Property(x => x.IsMilestone).HasColumnName(@"IsMilestone").IsOptional().HasColumnType("bit");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.DependsOn).HasColumnName(@"DependsOn").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.DueDateBase).HasColumnName(@"DueDateBase").IsOptional().HasColumnType("tinyint");
            Property(x => x.DueDateDiff).HasColumnName(@"DueDateDiff").IsOptional().HasColumnType("smallint");
            Property(x => x.ShortName).HasColumnName(@"ShortName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.IsCriticalPath).HasColumnName(@"IsCriticalPath").IsRequired().HasColumnType("bit");
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>
