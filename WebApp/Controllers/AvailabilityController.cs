using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.Enums;
using WebApp.ViewModels.AvailabilityViewModels;
using System.Globalization;
using WebApp.Library;

namespace WebApp.Controllers
{
    public class AvailabilityController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Availability
        public async Task<ActionResult> Index(string id)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            //TODO: this will not be limited to just physicians, intakes should be able to manage their availability as well.
            string physicianId;
            // Admins will be able to manage available days on behalf of others, other roles can only manage their own.
            if (user.RoleCategoryId == RoleCategory.Admin && !string.IsNullOrEmpty(id))
            {
                physicianId = id;
            }
            else
            {
                physicianId = user.Id;
            }

            var availableDays = await db.AvailableDays.Where(c => c.PhysicianId == physicianId).ToListAsync();

            var selectedUser = await db.Users.SingleOrDefaultAsync(c => c.Id == physicianId);

            var model = new IndexViewModel()
            {
                AvailableDays = availableDays,
                CurrentUser = user,
                SelectedUser = selectedUser,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = SystemTime.Now(),
                NewAvailableDay = new AvailableDay()
                {
                    PhysicianId = selectedUser.Id,
                    Day = SystemTime.Now()
                }
            };

            var arr = model.AvailableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray<string>();
            ViewBag.AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddDay(AvailableDay newDay)
        {
            if (ModelState.IsValid)
            {
                using (var db = new OrvosiEntities(User.Identity.Name))
                {
                    db.AvailableDays.Add(newDay);
                    await db.SaveChangesAsync();
                    //await db.Entry(obj).ReloadAsync();
                    return RedirectToAction("Index", new { id = newDay.PhysicianId });
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}