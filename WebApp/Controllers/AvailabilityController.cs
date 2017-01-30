using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orvosi.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using Orvosi.Shared.Enums;
using WebApp.ViewModels.AvailabilityViewModels;
using System.Globalization;
using WebApp.Library;
using System.Net;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class AvailabilityController : Controller
    {
        //OrvosiDbContext db = new OrvosiDbContext();
        Orvosi.Data.OrvosiDbContext context = new Orvosi.Data.OrvosiDbContext();

        // GET: Availability
        [AuthorizeRole(Feature = Features.Availability.ViewUnpublished)]
        public async Task<ActionResult> Index(FilterArgs args)
        {
            // if one of the params is set, both are required
            if ((args.Year.HasValue && !args.Month.HasValue) || (!args.Year.HasValue && args.Month.HasValue))
            {
                ModelState.AddModelError("", "Both the year and month must be set to a value");
                
                return View(new IndexViewModel
                {
                    Months = new List<MonthGroup>(),
                    FilterArgs = args
                });
            }

            var userContext = User.Identity.GetUserContext();

            var availableDays = context.AvailableDays
                .Where(c => c.PhysicianId == userContext.Id);

            if (args.Year.HasValue && args.Month.HasValue)
            {
                var selectedMonth = new DateTime(args.Year.Value, args.Month.Value, 1);
                var nextMonth = selectedMonth.AddMonths(1);
                availableDays = availableDays
                    .Where(c => c.Day >= selectedMonth && c.Day < nextMonth);
            }
            else
            {
                var thisMonth = new DateTime(SystemTime.Now().Year, SystemTime.Now().Month, 1);
                availableDays = availableDays
                    .Where(c => c.Day >= thisMonth);
            }

            var result = availableDays.ToList();

            var months = result
                .GroupBy(ad => new { Month = new DateTime(ad.Day.Year, ad.Day.Month, 1) })
                .Select(ad => new MonthGroup
                {
                    Month = ad.Key.Month,
                    AvailableDays = ad
                }).ToList();

            var model = new IndexViewModel()
            {
                Months = months,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = SystemTime.Now(),
                FilterArgs = args
            };

            return View(model);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> AddDay(Guid id)
        {
            var physician = context.AspNetUsers.Single(c => c.Id == id);

            var availableDays = await context.AvailableDays.Where(c => c.PhysicianId == id).ToListAsync();

            var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray<string>();
            ViewBag.AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));
            ViewBag.PhysicianName = physician.GetDisplayName();
            var model = new AvailableDay()
            {
                PhysicianId = id,
                Day = SystemTime.Now()
            };
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> AddDay(AvailableDay model)
        {
            if (ModelState.IsValid)
            {
                var dates = Request.Form["AvailableDays"].Split(',');
                foreach (var item in dates)
                {
                    var day = new Orvosi.Data.AvailableDay()
                    {
                        Day = DateTime.Parse(item),
                        PhysicianId = model.PhysicianId,
                        CompanyId = model.CompanyId,
                        LocationId = model.LocationId,
                        IsPrebook = model.IsPrebook
                    };
                    context.AvailableDays.Add(day);
                }
                await context.SaveChangesAsync();
                return RedirectToAction("Index", new { PhysicianId = model.PhysicianId });
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> AddSlots(short AvailableDayId, short StartHour, short StartMinute, short Duration, byte Repeat)
        {
            var firstStartTime = new TimeSpan(StartHour, StartMinute, 0);
                for (int i = 0; i < Repeat; i++)
                {
                    var startTime = firstStartTime.Add(new TimeSpan(0, Duration * i, 0));
                    var endTime = startTime.Add(new TimeSpan(0, Duration, 0));
                    var slot = new Orvosi.Data.AvailableSlot()
                    {
                        AvailableDayId = AvailableDayId,
                        StartTime = startTime,
                        EndTime = endTime,
                        Duration = Duration
                    };
                context.AvailableSlots.Add(slot);
                await context.SaveChangesAsync();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> CancelDay(int id)
        {
            var day = await context.AvailableDays.FirstAsync(c => c.Id == id);

            if (day.AvailableSlots.Any(a => a.ServiceRequests.Any()))
            {
                ModelState.AddModelError("", "Slots have already been booked.");
            }
            context.AvailableDays.Remove(day);
            await context.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> CancelSlot(int id)
        {
            var slot = await context.AvailableSlots.FirstAsync(c => c.Id == id);

            if (slot.ServiceRequests.Any())
            {
                ModelState.AddModelError("", "Slot has already been booked.");
            }
            context.AvailableSlots.Remove(slot);
            await context.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}