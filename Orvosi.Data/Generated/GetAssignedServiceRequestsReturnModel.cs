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

    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.21.1.0")]
    public partial class GetAssignedServiceRequestsReturnModel
    {
        public System.Int32 ServiceRequestId { get; set; }
        public System.DateTime? ReportDueDate { get; set; }
        public System.DateTime? AppointmentDate { get; set; }
        public System.TimeSpan? StartTime { get; set; }
        public System.DateTime? DueDate { get; set; }
        public System.Int16? CompanyId { get; set; }
        public System.String CompanyName { get; set; }
        public System.Int16? ServiceId { get; set; }
        public System.String ServiceName { get; set; }
        public System.Int16? ServiceCategoryId { get; set; }
        public System.String ClaimantName { get; set; }
        public System.Int32? AddressId { get; set; }
        public System.String AddressName { get; set; }
        public System.String City { get; set; }
        public System.String BoxCaseFolderId { get; set; }
        public System.String Title { get; set; }
        public System.String ServiceCode { get; set; }
        public System.String ServiceColorCode { get; set; }
        public System.Boolean? IsLateCancellation { get; set; }
        public System.Boolean? IsNoShow { get; set; }
        public System.DateTime? CancelledDate { get; set; }
        public System.Boolean? IsClosed { get; set; }
        public System.Byte? ServiceRequestStatusId { get; set; }
        public System.Int32 Id { get; set; }
        public System.Int16? TaskId { get; set; }
        public System.String TaskName { get; set; }
        public System.Int16? TaskSequence { get; set; }
        public System.Byte TaskStatusId { get; set; }
        public System.String TaskStatusName { get; set; }
        public System.String BoxCollaborationId { get; set; }
        public System.DateTime? CompletedDate { get; set; }
        public System.Boolean IsObsolete { get; set; }
        public System.Guid? ResponsibleRoleId { get; set; }
        public System.Byte? Workload { get; set; }
        public System.Guid? AssignedTo { get; set; }
        public System.String TaskType { get; set; }
        public System.String ResponsibleRoleName { get; set; }
        public System.String AssignedToDisplayName { get; set; }
        public System.String AssignedToColorCode { get; set; }
        public System.String AssignedToInitials { get; set; }
        public System.Guid? AssignedToRoleId { get; set; }
        public System.String AssignedToRoleName { get; set; }
        public System.String DependsOnCSV { get; set; }
    }

}
// </auto-generated>