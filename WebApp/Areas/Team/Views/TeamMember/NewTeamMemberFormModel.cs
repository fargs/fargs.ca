using System;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class NewTeamMemberFormModel
    {
        public NewTeamMemberFormModel(Guid physicianId)
        {
            PhysicianId = physicianId.ToString();
        }

        public string Email { get; set; }
        public string PhysicianId { get; set; }
    }
}