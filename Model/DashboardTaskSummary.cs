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
    
    public partial class DashboardTaskSummary
    {
        public Nullable<short> TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskPhaseName { get; set; }
        public string AssignedToDisplayName { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string ResponsibleRoleId { get; set; }
        public string ResponsibleRoleName { get; set; }
        public Nullable<int> TaskCount { get; set; }
        public string AssignedToUserId { get; set; }
    }
}
