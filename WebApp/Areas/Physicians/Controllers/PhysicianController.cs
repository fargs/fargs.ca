using ImeHub.Data;
using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Areas.Physicians.Views.Physician;
using WebApp.Areas.Shared;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.Views.Shared;
using Features = Orvosi.Shared.Enums.Features;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Physicians.Controllers
{
    public class PhysicianController : Controller
    {
        private DateTime now;
        private IIdentity identity;
        private ImeHubDbContext db;

        public PhysicianController(ImeHubDbContext db, DateTime now, IPrincipal principal)
        {
            this.now = now;
            this.identity = principal.Identity;
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public ActionResult Index(Guid? physicianId)
        {
            var list = new ListViewModel(physicianId, db, identity, now);

            ReadOnlyViewModel readOnly = null;
            if (physicianId.HasValue)
            {
                readOnly = new ReadOnlyViewModel(physicianId.Value, db, identity, now);
            }
            
            var viewModel = new IndexViewModel(list, readOnly, identity, now);

            if (Request.IsAjaxRequest())
            {
                return PartialView(viewModel);
            }
            return View(viewModel);
        }

        #region Views

        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public PartialViewResult List(Guid? physicianId)
        {
            var viewModel = new ListViewModel(physicianId, db, identity, now);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public PartialViewResult ReadOnly(Guid physicianId)
        {
            var readOnly = new ReadOnlyViewModel(physicianId, db, identity, now);

            return PartialView(readOnly);
        }

        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public PartialViewResult ShowNewPhysicianForm()
        {
            var formModel = new NewPhysicianFormModel();

            return PartialView("NewPhysicianForm", formModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public async Task<ActionResult> SaveNewPhysicianForm(NewPhysicianFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("NewPhysicianForm", form);
            }

            var physician = new Physician
            {
                Id = Guid.NewGuid(),
                CompanyName = form.CompanyName,
                ManagerId = identity.GetGuidUserId()
            };
            db.Physicians.Add(physician);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = physician.Id
            });
        }

        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public PartialViewResult OwnerInvitationForm(Guid physicianId)
        {
            var formModel = new OwnerInvitationFormModel(physicianId, identity);

            return PartialView(formModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public async Task<ActionResult> SaveOwnerInvitationForm(OwnerInvitationFormModel form)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("OwnerInvitationForm", form);
            }

            var invite = new PhysicianInvite
            {
                Id = Guid.NewGuid(),
                PhysicianId = form.PhysicianId,
                ToEmail = form.ToEmail,
                ToName = form.ToName,
                FromEmail = form.FromEmail,
                FromName = form.FromName,
                AcceptanceStatusId = (byte)Enums.AcceptanceStatus.NotResponded
            };
            db.PhysicianInvites.Add(invite);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = invite.Id
            });
        }

        [AuthorizeRole(Feature = Features.SysAdmin.Section)]
        public PartialViewResult ShowDeleteConfirmation(Guid physicianId)
        {
            var formModel = new PhysicianFormModel(physicianId, db);

            return PartialView("DeleteConfirmation", formModel);
        }

        #endregion
    }
}