using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Areas.Team.Views.TeamMember;

namespace WebApp.Areas.Team.Controllers
{
    public class TeamMemberController : BaseController
    {
        private OrvosiDbContext db;

        public TeamMemberController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.Collaborator.Search)]
        public ViewResult Index(Guid? companyId)
        {
            var list = new ListViewModel(companyId, db, identity, now);

            var viewModel = new IndexViewModel(list, identity, now);

            return View(viewModel);
        }

        #region Views


        public PartialViewResult ShowNewTeamMemberForm()
        {
            var formModel = new NewTeamMemberFormModel(physicianId.Value);

            return PartialView("NewTeamMemberForm", formModel);
        }
        #endregion

    }
}