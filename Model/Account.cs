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
    
    public partial class Account
    {
        public System.Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public Nullable<short> CompanyId { get; set; }
        public string CompanySubmittedName { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public bool IsTestRecord { get; set; }
        public Nullable<System.Guid> RoleId { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public bool EmailConfirmed { get; set; }
        public Nullable<System.DateTime> LastActivationDate { get; set; }
    }
}
