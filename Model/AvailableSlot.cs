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
    
    public partial class AvailableSlot
    {
        public short Id { get; set; }
        public short AvailableDayId { get; set; }
        public System.TimeSpan StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<short> Duration { get; set; }
        public Nullable<short> ServiceRequestId { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string Title { get; set; }
    
        public virtual AvailableDay AvailableDay { get; set; }
    }
}
