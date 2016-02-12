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
using System.Net;

namespace WebApp.Controllers
{
    [Authorize]
    public class AvailabilityController : Controller
    {
        OrvosiEntities db = new OrvosiEntities();

        // GET: Availability
        public async Task<ActionResult> Index(FilterArgs args)
        {
            var thisMonth = new DateTime(SystemTime.Now().Year, SystemTime.Now().Month, 1);
            var nextMonth = thisMonth.AddMonths(1);
            if (args.Year.HasValue && args.Month.HasValue)
            {
                thisMonth = new DateTime(args.Year.Value, args.Month.Value, 1);
                nextMonth = thisMonth.AddMonths(1);
            }
            args.Year = thisMonth.Year;
            args.Month = thisMonth.Month;
            args.FilterDate = thisMonth;

            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            //TODO: this will not be limited to just physicians, intakes should be able to manage their availability as well.
            // Admins will be able to manage available days on behalf of others, other roles can only manage their own.
            if (user.RoleCategoryId == RoleCategory.Admin && !string.IsNullOrEmpty(args.PhysicianId))
            {
                args.PhysicianId = args.PhysicianId;
            }
            else
            {
                args.PhysicianId= user.Id;
            }

            var availableDays = await db.AvailableDays
                .Where(c => c.PhysicianId == args.PhysicianId && (c.Day >= thisMonth && c.Day < nextMonth))
                .ToListAsync();

            var selectedUser = await db.Users.SingleOrDefaultAsync(c => c.Id == args.PhysicianId);

            var model = new IndexViewModel()
            {
                AvailableDays = availableDays,
                CurrentUser = user,
                SelectedUser = selectedUser,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = SystemTime.Now(),
                FilterArgs = args
            };
            
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> AddDay(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var physician = db.Users.Single(c => c.Id == id);

            var availableDays = await db.AvailableDays.Where(c => c.PhysicianId == id).ToListAsync();

            var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray<string>();
            ViewBag.AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));

            var model = new AvailableDay()
            {
                PhysicianId = id,
                PhysicianName = physician.DisplayName,
                Day = SystemTime.Now()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddDay(AvailableDay model)
        {
            var user = db.Users.Single(u => u.UserName == User.Identity.Name);

            // Admins will be able to manage available days on behalf of others, other roles can only manage their own.
            if (user.RoleCategoryId != RoleCategory.Admin && model.PhysicianId != user.Id)
            {
                throw new Exception("You are not allowed to update this users information.");
            }

            if (ModelState.IsValid)
            {
                using (var db = new OrvosiEntities(User.Identity.Name))
                {
                    var dates = Request.Form["AvailableDays"].Split(',');
                    foreach (var item in dates)
                    {
                        var day = new AvailableDay()
                        {
                            Day = DateTime.Parse(item),
                            PhysicianId = model.PhysicianId,
                            CompanyId = model.CompanyId,
                            LocationId = model.LocationId,
                            IsPrebook = model.IsPrebook
                        };
                        db.AvailableDays.Add(day);
                    }
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index", new { id = model.PhysicianId });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSlots(short AvailableDayId, short StartHour, short StartMinute, short Duration, byte Repeat)
        {
            var firstStartTime = new TimeSpan(StartHour, StartMinute, 0);
            using (var db = new OrvosiEntities(User.Identity.Name))
            {
                for (int i = 0; i < Repeat; i++)
                {
                    var startTime = firstStartTime.Add(new TimeSpan(0, Duration * i, 0));
                    var endTime = startTime.Add(new TimeSpan(0, Duration, 0));
                    var obj = new AvailableSlot()
                    {
                        AvailableDayId = AvailableDayId,
                        StartTime = startTime,
                        EndTime = endTime,
                        Duration = Duration,
                        Title = string.Empty // Needed because the view makes this non nullable.
                    };
                    db.AvailableSlots.Add(obj);
                }
                await db.SaveChangesAsync();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public async Task<ActionResult> CancelDay(int id)
        {
            var day = await db.AvailableDays.SingleOrDefaultAsync(c => c.Id == id);
            if (day == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (day.AvailableSlots.FirstOrDefault(c => c.ServiceRequestId.HasValue) != null)
            {
                ModelState.AddModelError("", "Slots have already been booked.");
            }

            db.AvailableDays.Remove(day);

            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public async Task<ActionResult> CancelSlot(int id)
        {
            var slot = await db.AvailableSlots.SingleOrDefaultAsync(c => c.Id == id);
            if (slot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (slot.ServiceRequestId != null)
            {
                ModelState.AddModelError("", "Slot has already been booked.");
            }

            db.AvailableSlots.Remove(slot);

            await db.SaveChangesAsync();

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}