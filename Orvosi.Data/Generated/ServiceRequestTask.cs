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
    public partial class ServiceRequestTask
    {
        public int Id { get; set; } // Id (Primary key)
        public System.Guid ObjectGuid { get; set; } // ObjectGuid
        public int ServiceRequestId { get; set; } // ServiceRequestId
        public short? TaskId { get; set; } // TaskId
        public string TaskName { get; set; } // TaskName (length: 128)
        public System.Guid? ResponsibleRoleId { get; set; } // ResponsibleRoleId
        public string ResponsibleRoleName { get; set; } // ResponsibleRoleName (length: 128)
        public short? Sequence { get; set; } // Sequence
        public System.Guid? AssignedTo { get; set; } // AssignedTo
        public bool IsBillable { get; set; } // IsBillable
        public decimal? HourlyRate { get; set; } // HourlyRate
        public decimal? EstimatedHours { get; set; } // EstimatedHours
        public decimal? ActualHours { get; set; } // ActualHours
        public System.DateTime? CompletedDate { get; set; } // CompletedDate
        public string Notes { get; set; } // Notes (length: 2000)
        public short? InvoiceItemId { get; set; } // InvoiceItemId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public byte? TaskPhaseId { get; set; } // TaskPhaseId
        public string TaskPhaseName { get; set; } // TaskPhaseName (length: 128)
        public string Guidance { get; set; } // Guidance (length: 1000)
        public bool IsObsolete { get; set; } // IsObsolete
        public string DependsOn { get; set; } // DependsOn (length: 50)
        public byte? DueDateBase { get; set; } // DueDateBase
        public short? DueDateDiff { get; set; } // DueDateDiff
        public string ShortName { get; set; } // ShortName (length: 50)
        public bool IsCriticalPath { get; set; } // IsCriticalPath
        public bool? IsDependentOnExamDate { get; private set; } // IsDependentOnExamDate
        public byte? Workload { get; set; } // Workload
        public System.Guid? ServiceRequestTemplateTaskId { get; set; } // ServiceRequestTemplateTaskId
        public string TaskType { get; set; } // TaskType (length: 20)
        public System.Guid? CompletedBy { get; set; } // CompletedBy
        public System.DateTime? DueDate { get; set; } // DueDate
        public short TaskStatusId { get; set; } // TaskStatusId
        public System.Guid? TaskStatusChangedBy { get; set; } // TaskStatusChangedBy
        public System.DateTime? TaskStatusChangedDate { get; set; } // TaskStatusChangedDate
        public System.DateTime? CreatedDate { get; set; } // CreatedDate
        public string CreatedUser { get; set; } // CreatedUser (length: 100)
        public System.DateTime? EffectiveDate { get; set; } // EffectiveDate
        public byte? EffectiveDateBase { get; set; } // EffectiveDateBase
        public short? EffectiveDateDiff { get; set; } // EffectiveDateDiff

        // Reverse navigation
        public virtual System.Collections.Generic.ICollection<ServiceRequestTask> Child { get; set; } // Many to many mapping
        public virtual System.Collections.Generic.ICollection<ServiceRequestTask> Parent { get; set; } // Many to many mapping

        // Foreign keys
        public virtual AspNetUser AspNetUser_AssignedTo { get; set; } // FK_ServiceRequestTask_AspNetUsers
        public virtual AspNetUser AspNetUser_CompletedBy { get; set; } // FK_ServiceRequestTask_AspNetUsers1
        public virtual AspNetUser AspNetUser_TaskStatusChangedBy { get; set; } // FK_ServiceRequestTask_AspNetUsers_TaskStatusChangedBy
        public virtual OTask OTask { get; set; } // FK_ServiceRequestTask_Task
        public virtual ServiceRequest ServiceRequest { get; set; } // FK_ServiceRequestTask_ServiceRequest
        public virtual ServiceRequestTemplateTask ServiceRequestTemplateTask { get; set; } // FK_ServiceRequestTask_ServiceRequestTemplateTask

        public ServiceRequestTask()
        {
            ObjectGuid = System.Guid.NewGuid();
            IsBillable = false;
            ModifiedDate = System.DateTime.Now;
            ModifiedUser = "suser_name()";
            IsObsolete = false;
            IsCriticalPath = false;
            TaskStatusId = 2;
            Child = new System.Collections.Generic.List<ServiceRequestTask>();
            Parent = new System.Collections.Generic.List<ServiceRequestTask>();
            InitializePartial();
        }

        partial void InitializePartial();
    }

}
// </auto-generated>
