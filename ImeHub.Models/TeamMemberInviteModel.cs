using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data = ImeHub.Data;

namespace ImeHub.Models
{
    public class TeamMemberInviteModel
    {
        public System.Guid Id { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Enums.InviteStatus InviteStatusId { get; set; }
        public System.DateTime InviteStatusChangedDate { get; set; }
        public System.Guid InviteStatusChangedBy { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.Guid RoleId { get; set; }
        public System.Guid PhysicianId { get; set; }

        public virtual LookupModel<byte> InviteStatus { get; set; }

        public virtual PhysicianModel Physician { get; set; }

        public virtual RoleModel Role { get; set; }

        // Extra
        public string DisplayName => $"{Title}{(!string.IsNullOrEmpty(Title) ? " " : "")}{FirstName} {LastName}";

        public static Expression<Func<Data.TeamMemberInvite, TeamMemberInviteModel>> FromTeamMemberInvite = a => a == null ? null : new TeamMemberInviteModel
        {
            Id = a.Id,
            To = a.To,
            Cc = a.Cc,
            Bcc = a.Bcc,
            Subject = a.Subject,
            Body = a.Body,
            InviteStatusId = (Enums.InviteStatus)a.InviteStatusId,
            InviteStatusChangedDate = a.InviteStatusChangedDate,
            InviteStatusChangedBy = a.InviteStatusChangedBy,
            Title = a.Title,
            FirstName = a.FirstName,
            LastName = a.LastName,
            RoleId = a.RoleId,
            PhysicianId = a.PhysicianId,
            InviteStatus = LookupModel<byte>.FromInviteStatus.Invoke(a.InviteStatu),
            Physician = PhysicianModel.FromPhysician.Invoke(a.Physician),
            Role = RoleModel.FromRole.Invoke(a.Role),
        };
    }
}
