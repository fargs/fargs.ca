//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceRequestTask
    {
        public int Id { get; set; }
        public System.Guid ObjectGuid { get; set; }
        public int ServiceRequestId { get; set; }
        public string TaskName { get; set; }
        public string ResponsibleRoleId { get; set; }
        public string ResponsibleRoleName { get; set; }
        public Nullable<short> Sequence { get; set; }
        public string AssignedTo { get; set; }
        public bool IsBillable { get; set; }
        public Nullable<decimal> EstimatedHours { get; set; }
        public Nullable<decimal> ActualHours { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string Notes { get; set; }
        public Nullable<short> InvoiceItemId { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string AssignedToDisplayName { get; set; }
        public Nullable<byte> TaskPhaseId { get; set; }
        public string TaskPhaseName { get; set; }
        public Nullable<short> TaskId { get; set; }
        public string Guidance { get; set; }
        public Nullable<decimal> StaffHourlyRate { get; set; }
        public Nullable<decimal> HourlyRate { get; set; }
        public string UserId { get; set; }
        public Nullable<decimal> Cost { get; set; }
    }
}
