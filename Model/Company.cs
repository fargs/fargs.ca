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
    
    public partial class Company
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string LogoCssClass { get; set; }
        public string MasterBookingPageByPhysician { get; set; }
        public string MasterBookingPageByTime { get; set; }
        public string MasterBookingPageTeleconference { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string ParentName { get; set; }
        public Nullable<System.Guid> ObjectGuid { get; set; }
        public bool IsParent { get; set; }
        public string Code { get; set; }
    }
}