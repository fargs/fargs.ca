using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Views.Shared;

namespace WebApp.Areas.Team.Views.TeamMember
{
    public class InviteTeamMemberFormModel : ViewModelBase
    {
        public InviteTeamMemberFormModel()
        {
        }
        public InviteTeamMemberFormModel(Guid physicianId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            PhysicianId = physicianId;
            ViewData = new ViewDataModel(db, physicianId);
        }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DisplayName => $"{Title}{(string.IsNullOrEmpty(Title) ? "" : " ")}{FirstName} {LastName}";

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;
            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;
                var physician = db.Physicians.Where(p => p.Id == physicianId).Select(LookupModel<Guid>.FromPhysician.Expand()).Single();
                Physician = LookupViewModel<Guid>.FromLookupModel.Invoke(physician);
                Roles = GetRoleSelectList();
            }
            public LookupViewModel<Guid> Physician { get; set; }
            public IEnumerable<SelectListItem> Roles { get; set; }

            private List<SelectListItem> GetRoleSelectList()
            {
                var roles = db.TeamRoles
                    .Select(LookupModel<Guid>.FromTeamRole.Expand())
                    .ToList();

                return roles
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
        }
    }
}