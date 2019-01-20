using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Library.Filters;
using Enums = ImeHub.Models.Enums;
using Features = ImeHub.Models.Enums.Features;
using WebApp.Areas.Team.Views.TeamMember;
using ImeHub.Data;
using WebApp.Library.Extensions;
using System.Net.Mail;
using WebApp.Library.Helpers;


namespace WebApp.Areas.Team.Controllers
{
    public class TeamMemberController : BaseController
    {
        private ImeHubDbContext db;

        public TeamMemberController(ImeHubDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.PhysicianPortal.Team.Search)]
        public ViewResult Index(Guid? selectedTeamMemberId)
        {
            var list = new ListViewModel(selectedTeamMemberId, db, identity, now);

            var viewModel = new IndexViewModel(list, identity, now);

            return View(viewModel);
        }

        #region Views


        public PartialViewResult ShowInviteTeamMemberForm()
        {
            var formModel = new InviteTeamMemberFormModel(physicianId.Value, db, identity, now);

            return PartialView("InviteTeamMemberForm", formModel);
        }

        public async System.Threading.Tasks.Task<ActionResult> SaveInviteTeamMemberForm(InviteTeamMemberFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("InviteTeamMemberForm", form);
            }

            var invite = new TeamMemberInvite
            {
                Id = Guid.NewGuid(),
                To = form.Email,
                Subject = $"Invite to join {this.physicianContext.Name}",
                Title = form.Title,
                FirstName = form.FirstName,
                LastName = form.LastName,
                RoleId = form.RoleId,
                PhysicianId = this.physicianId.Value,
                InviteStatusId = (byte)Enums.InviteStatus.NotSent,
                InviteStatusChangedBy = loggedInUserId,
                InviteStatusChangedDate = now
            };

            var physician = db.Physicians.Single(p => p.Id == form.PhysicianId);
            var role = db.Roles.Single(r => r.Id == form.RoleId);

            var notification = new TeamMemberInvitationNotificationViewModel
            {
                InviteId = invite.Id.ToString(),
                Invitee = form.DisplayName,
                PhysicianCompanyName = physician.CompanyName,
                RoleName = role.Name
            };
            ViewData["BaseUrl"] = Url.Content("~"); //This is needed because the full address needs to be included in the email download link
            invite.Body = HtmlHelpers.RenderPartialViewToString(this, "TeamMemberInvitationNotification", notification);
            
            db.TeamMemberInvites.Add(invite);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = invite.Id
            });
        }
        #endregion
    }
}