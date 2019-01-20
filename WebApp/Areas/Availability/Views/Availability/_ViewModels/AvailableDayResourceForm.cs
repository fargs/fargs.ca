using ImeHub.Data;
using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayResourceForm : ViewModelBase
    {
        public AvailableDayResourceForm()
        {
        }
        public AvailableDayResourceForm(ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(db, PhysicianId.Value);
        }
        [Required]
        public Guid AvailableDayId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                TeamMembers = GetTeamMembersSelectList(physicianId);
            }
            public IEnumerable<SelectListItem> TeamMembers { get; set; }

            private List<SelectListItem> GetTeamMembersSelectList(Guid physicianId)
            {
                var teamMembers = db.TeamMembers
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(tm => tm.PhysicianId == physicianId)
                    .Select(TeamMemberModel.FromTeamMember)
                    .ToList();

                return teamMembers
                    .Select(d => new SelectListItem
                    {
                        Text = d.User.DisplayName,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
        }
    }
}