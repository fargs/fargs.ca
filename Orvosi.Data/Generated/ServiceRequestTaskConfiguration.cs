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

    // ServiceRequestTask
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class ServiceRequestTaskConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<ServiceRequestTask>
    {
        public ServiceRequestTaskConfiguration()
            : this("dbo")
        {
        }

        public ServiceRequestTaskConfiguration(string schema)
        {
            ToTable(schema + ".ServiceRequestTask");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            Property(x => x.ObjectGuid).HasColumnName(@"ObjectGuid").IsRequired().HasColumnType("uniqueidentifier");
            Property(x => x.ServiceRequestId).HasColumnName(@"ServiceRequestId").IsRequired().HasColumnType("int");
            Property(x => x.TaskId).HasColumnName(@"TaskId").IsOptional().HasColumnType("smallint");
            Property(x => x.TaskName).HasColumnName(@"TaskName").IsRequired().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ResponsibleRoleId).HasColumnName(@"ResponsibleRoleId").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.ResponsibleRoleName).HasColumnName(@"ResponsibleRoleName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Sequence).HasColumnName(@"Sequence").IsOptional().HasColumnType("smallint");
            Property(x => x.AssignedTo).HasColumnName(@"AssignedTo").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.IsBillable).HasColumnName(@"IsBillable").IsRequired().HasColumnType("bit");
            Property(x => x.HourlyRate).HasColumnName(@"HourlyRate").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.EstimatedHours).HasColumnName(@"EstimatedHours").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.ActualHours).HasColumnName(@"ActualHours").IsOptional().HasColumnType("decimal").HasPrecision(18,2);
            Property(x => x.CompletedDate).HasColumnName(@"CompletedDate").IsOptional().HasColumnType("datetime");
            Property(x => x.Notes).HasColumnName(@"Notes").IsOptional().HasColumnType("nvarchar").HasMaxLength(2000);
            Property(x => x.InvoiceItemId).HasColumnName(@"InvoiceItemId").IsOptional().HasColumnType("smallint");
            Property(x => x.ModifiedDate).HasColumnName(@"ModifiedDate").IsRequired().HasColumnType("datetime");
            Property(x => x.ModifiedUser).HasColumnName(@"ModifiedUser").IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.TaskPhaseId).HasColumnName(@"TaskPhaseId").IsOptional().HasColumnType("tinyint");
            Property(x => x.TaskPhaseName).HasColumnName(@"TaskPhaseName").IsOptional().HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Guidance).HasColumnName(@"Guidance").IsOptional().HasColumnType("nvarchar").HasMaxLength(1000);
            Property(x => x.IsObsolete).HasColumnName(@"IsObsolete").IsRequired().HasColumnType("bit");
            Property(x => x.DependsOn).HasColumnName(@"DependsOn").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.DueDateBase).HasColumnName(@"DueDateBase").IsOptional().HasColumnType("tinyint");
            Property(x => x.DueDateDiff).HasColumnName(@"DueDateDiff").IsOptional().HasColumnType("smallint");
            Property(x => x.ShortName).HasColumnName(@"ShortName").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.IsCriticalPath).HasColumnName(@"IsCriticalPath").IsRequired().HasColumnType("bit");
            Property(x => x.IsDependentOnExamDate).HasColumnName(@"IsDependentOnExamDate").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            // Foreign keys
            HasOptional(a => a.AspNetUser).WithMany(b => b.ServiceRequestTasks).HasForeignKey(c => c.AssignedTo).WillCascadeOnDelete(false); // FK_ServiceRequestTask_AspNetUsers
            HasOptional(a => a.Task).WithMany(b => b.ServiceRequestTasks).HasForeignKey(c => c.TaskId).WillCascadeOnDelete(false); // FK_ServiceRequestTask_Task
            HasRequired(a => a.ServiceRequest).WithMany(b => b.ServiceRequestTasks).HasForeignKey(c => c.ServiceRequestId); // FK_ServiceRequestTask_ServiceRequest
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
// </auto-generated>