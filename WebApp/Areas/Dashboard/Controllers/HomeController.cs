using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Dashboard.Views.Home;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private DateTime now;
        private IIdentity identity;
        private Guid userId;
        private ImeHubDbContext db;

        public HomeController(ImeHubDbContext db, IIdentity identity, DateTime now)
        {
            this.now = now;
            this.identity = identity;
            this.userId = identity.GetGuidUserId();
            this.db = db;
        }
        public ActionResult Index(Guid? physicianId)
        {
            var data = UserModel.FromUser.Invoke(db.Users.AsNoTracking().Single(u => u.Id == userId));

            var viewModel = new IndexViewModel(data);

            if (Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            return View(viewModel);
        }


        public ActionResult AcceptOwnershipForm(Guid inviteId)
        {
            var invite = db.PhysicianOwners.SingleOrDefault(pi => pi.PhysicianId == inviteId);
            // check if invite exists
            if (invite == null)
            {
                return Redirect("InvitationNotValid");
            }
            // check if invite is for the authenticated user
            if (invite.Email != User.Identity.GetEmail())
            {
                return Redirect("InvitationNotValid");
            }

            var viewModel = new AcceptOwnershipFormModel(inviteId);
            return PartialView(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AcceptOwnership(AcceptOwnershipFormModel form)
        {
            var owner = db.PhysicianOwners.SingleOrDefault(pi => pi.PhysicianId == form.OwnerId);
            // check if invite exists
            if (owner == null)
            {
                return Redirect("InvitationNotValid");
            }
            // check if invite is for the authenticated user
            if (owner.Email != User.Identity.GetEmail())
            {
                return Redirect("InvitationNotValid");
            }

            var physician = db.Physicians.Single(p => p.Id == owner.PhysicianId);
            physician.OwnerId = User.Identity.GetGuidUserId();

            await db.SaveChangesAsync();

            return Redirect("~/Home/Index");
        }

    }
}