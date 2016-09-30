using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.AccountingModel;
using WebApp.Library.Extensions;

namespace WebApp.Controllers
{
    public class AccountingController : Controller
    {
        // GET: Accounting
        public ActionResult Today(Guid? serviceProviderId)
        {
            Guid userId = User.Identity.GetGuidUserId();
            // Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            if (User.Identity.IsAdmin() && serviceProviderId.HasValue)
            {
                userId = serviceProviderId.Value;
            }

            var model = new Mapper(new Orvosi.Data.OrvosiDbContext()).MapToToday(userId, SystemTime.Now());

            ViewBag.SelectedUserId = userId;

            return View(model);
        }
    }
}