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
    
    public partial class PhysicianLocation
    {
        public short Id { get; set; }
        public string PhysicianId { get; set; }
        public Nullable<short> LocationId { get; set; }
        public bool IsPrimary { get; set; }
        public Nullable<byte> Preference { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
    }
}
