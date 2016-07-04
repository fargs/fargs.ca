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

    // ServiceRequestTemplateTask
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestTemplateTaskConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceRequestTemplateTask>
    {
        public ServiceRequestTemplateTaskConfiguration()
            : this("dbo")
        {
        }

        public ServiceRequestTemplateTaskConfiguration(string schema)
        {
            ToTable(schema + ".ServiceRequestTemplateTask");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("smallint").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ServiceRequestTemplateId).HasColumnName(@"ServiceRequestTemplateId").IsRequired().HasColumnType("smallint");
            Property(x => x.TaskPhaseId).HasColumnName(@"TaskPhaseId").IsOptional().HasColumnType("tinyint");
            Property(x => x.TaskName).HasColumnName(@"TaskName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Guidance).HasColumnName(@"Guidance").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.Sequence).HasColumnName(@"Sequence").IsOptional().HasColumnType("smallint");
            Property(x => x.IsBillable).HasColumnName(@"IsBillable").IsRequired().HasColumnType("bit");
            Property(x => x.EstimatedHours).HasColumnName(@"EstimatedHours").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.HourlyRate).HasColumnName(@"HourlyRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.ResponsibleRoleId).HasColumnName(@"ResponsibleRoleId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.DependsOn).HasColumnName(@"DependsOn").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.ParentId).HasColumnName(@"ParentId").IsOptional().HasColumnType("smallint");
            Property(x => x.TaskId).HasColumnName(@"TaskId").IsOptional().HasColumnType("smallint");

            // Foreign keys
            HasOptional(a => a.Task).WithMany(b => b.ServiceRequestTemplateTasks).HasForeignKey(c => c.TaskId).WillCascadeOnDelete(false); // FK_ServiceRequestTemplateTask_Task
            HasRequired(a => a.ServiceRequestTemplate).WithMany(b => b.ServiceRequestTemplateTasks).HasForeignKey(c => c.ServiceRequestTemplateId).WillCascadeOnDelete(false); // FK_ServiceRequestTemplateTask_ServiceRequestTemplate
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>