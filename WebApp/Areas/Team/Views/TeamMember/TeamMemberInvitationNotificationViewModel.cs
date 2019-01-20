using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class TeamMemberInvitationNotificationViewModel
    {
        public string Invitee { get; set; }
        public string RoleName { get; set; }
        public string PhysicianCompanyName { get; set; }
        public string PhysicianOwnerName { get; set; }
        public string InviteId { get; set; }
    }
}