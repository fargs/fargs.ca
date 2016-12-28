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
using System.Diagnostics;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Super Admin, Case Coordinator")]
    public class AvailabilityController : Controller
    {
        //OrvosiDbContext db = new OrvosiDbContext();
        Orvosi.Data.OrvosiDbContext context = new Orvosi.Data.OrvosiDbContext();

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

            //TODO: this will not be limited to just physicians, intakes should be able to manage their availability as well.
            // Admins will be able to manage available days on behalf of others, other roles can only manage their own.
            if ((User.Identity.GetRoleId() == AspNetRoles.SuperAdmin || User.Identity.GetRoleId() == AspNetRoles.CaseCoordinator) && args.PhysicianId.HasValue)
            {
                args.PhysicianId = args.PhysicianId;
            }
            else
            {
                args.PhysicianId = User.Identity.GetGuidUserId();
            }

            var availableDays = context.AvailableDays
                .Where(c => c.PhysicianId == args.PhysicianId && (c.Day >= thisMonth && c.Day < nextMonth));

            var selectedUser = await context.AspNetUsers.SingleOrDefaultAsync(c => c.Id == args.PhysicianId);

            var model = new IndexViewModel()
            {
                AvailableDays = availableDays,
                SelectedUser = selectedUser,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = SystemTime.Now(),
                FilterArgs = args
            };

            return View(model);
        }

        [HttpGet]
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
        public async Task<ActionResult> AssignResources(FormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                //Take post variables and place into local variables
                short id = Convert.ToInt16(Request.Form["AvailabilityId"]);
                //looks for all intake attached to this available day
                var emptyResources = context.AvailableDayResources
                                        .Where(c => c.AvailableDayId == id)
                                        .Select(x => x.UserId);
                //makes sure that selected list can be null (delete all intakes attached to that date)
                string [] assistants = null;                
                if (Request.Form["list"] != null)
                {
                    assistants = Request.Form["list"].Split(',');
                }
                List<string> exist = new List<string>();
                if (assistants != null)
                {
                    foreach (var item in assistants)
                    {
                        //checks is selected item is already within DB
                        //handles data insertion functions
                        var oldResources = context.AvailableDayResources
                                            .Where(c => c.AvailableDayId == id && c.UserId == new Guid(item))
                                            .FirstOrDefault();
                        if (oldResources == null)
                        {
                            var resource = new Orvosi.Data.AvailableDayResource()
                            {
                                AvailableDayId = id,
                                UserId = new Guid(item)
                            };
                            context.AvailableDayResources.Add(resource);
                        }
                        //populates local array with the resources that exist in both the DB and selected items
                        foreach (var empty in emptyResources)
                        {
                            if (empty.ToString() == item)
                            {
                                exist.Add(empty.ToString());
                                break;
                            }
                        }
                    }
                }
                //checks through all DB resources and sees if they exist in "exist" array populated just above code
                //handles data removal functions
                foreach (var empty1 in emptyResources)
                {
                    if (!exist.Contains(empty1.ToString()))
                    {
                        var resource = await context.AvailableDayResources
                            .FirstAsync(c => c.AvailableDayId == id && c.UserId == empty1);
                        context.AvailableDayResources.Remove(resource);
                    }
                }
                // await until changes are synced
                await context.SaveChangesAsync();
            }     
            //refresh page      
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
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