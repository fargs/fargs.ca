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
    
    public partial class MyTask
    {
        public string AssignedTo { get; set; }
        public int ServiceRequestId { get; set; }
        public Nullable<short> TaskId { get; set; }
        public Nullable<byte> TaskStatus { get; set; }
        public bool IsObsolete { get; set; }
        public Nullable<System.DateTime> DependentCompletedDate { get; set; }
        public Nullable<bool> DependentIsObsolete { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
    }
}