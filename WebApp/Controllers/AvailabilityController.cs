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
                SelectedUser = user,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = SystemTime.Now()
            };

            return View(model);
        }
    }
}