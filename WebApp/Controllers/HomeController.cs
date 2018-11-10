using ImeHub.Data;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;

namespace WebApp.Controllers
{
    [Authorize()]
    public class HomeController : Controller
    {
        private DateTime now;
        private ImeHubDbContext db;
        public HomeController(ImeHubDbContext db, DateTime now)
        {
            this.now = now;
            this.db = db;
        }

        public ActionResult Index()
        {
            // eventually this will be a public marketing page
            return Portal(null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="physicianId">
        /// This is expected to have a value when a user navigates to a physician's portal from 
        /// a link within the application. The value is expected to be null when the user logs in.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public ActionResult Portal(Guid? physicianId)
        {
            var physicianIdFromContext = User.Identity.GetPhysicianId();
            var userId = User.Identity.GetGuidUserId();
            var userModel = db.Users
                .AsNoTracking()
                .AsExpandable()
                .Where(u => u.Id == userId)
                .Select(ImeHub.Models.UserModel.FromUser)
                .SingleOrDefault();

            if (physicianId.HasValue && physicianId.Value == physicianIdFromContext)
            {
                return NavigateToPortal(physicianId.Value);
            }
            if (userModel.AsOwner != null)
            {
                return RedirectToActionPermanent("ChangeUserContextAsync", "Account", new { physicianId = userModel.AsOwner.Id });
            }

            return NavigateToDashboard();
        }

        private RedirectResult NavigateToDashboard()
        {
            return Redirect("~/Dashboard/Home");
        }
        private RedirectResult NavigateToPortal(Guid? physicianId)
        {
            var appointment = true; // db.AvailableDays.FirstOrDefault(ad => ad.PhysicianId == physicianId && ad.Day == now);
            if (appointment == null)
            {
                return Redirect("~/Work/Tasks");
            }
            else
            {
                return Redirect("~/Work/DaySheet");
            }
        }

        public ViewResult Unauthorized()
        {
            return View();
        }
    }
}