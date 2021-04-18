using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Companies
{
    public class CompanyUserInvitation
    {
        public long Id { get; set; }
        public Guid ObjectGuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public short CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyRoleId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime InvitedDate { get; set; }
        public Guid InvitedBy { get; set; }
        public string InviteCode { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? AcceptedDate { get; set; }
    }
}
