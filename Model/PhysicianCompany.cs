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
        public System.Guid PhysicianId { get; set; }
        public short CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LogoCssClass { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string ParentCompanyName { get; set; }
        public Nullable<byte> RelationshipStatusId { get; set; }
        public string RelationshipStatusName { get; set; }
        public string PhysicianDisplayName { get; set; }
        public Nullable<short> Id { get; set; }
        public Nullable<short> LookupId { get; set; }
        public string Text { get; set; }
        public Nullable<short> Value { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string ShortText { get; set; }
    }
}
