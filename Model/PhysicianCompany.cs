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
    
    public partial class PhysicianCompany
    {
        public string PhysicianId { get; set; }
        public short CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LogoCssClass { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string ParentCompanyName { get; set; }
        public byte RelationshipStatusId { get; set; }
        public string RelationshipStatusName { get; set; }
        public string PhysicianDisplayName { get; set; }
    }
}
