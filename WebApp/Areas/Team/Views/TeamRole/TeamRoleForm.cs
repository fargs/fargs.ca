using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Team.Views.TeamRole
{
    public class TeamRoleForm
    {
        public TeamRoleForm() { }
        public TeamRoleForm(Guid physicianId)
        {
            PhysicianId = physicianId;
        }
        public TeamRoleForm(Guid teamRoleId, Guid physicianId, ImeHubDbContext db) : this(physicianId)
        {
            var teamRole = db.TeamRoles
                .Single(s => s.Id == teamRoleId);

            TeamRoleId = teamRoleId;
            Name = teamRole.Name;
            PhysicianId = physicianId;
        }

        public Guid? TeamRoleId { get; set; }
        [Required]
        public Guid PhysicianId { get; set; }
        public string Name { get; set; }
    }
}