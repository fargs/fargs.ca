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
using WebApp.Models;
using LinqKit;
using WebApp.FormModels;
using WebApp.ViewModels;
using System.Security.Principal;

namespace WebApp.Controllers
{
    public class AvailabilityController : BaseController
    {
        private OrvosiDbContext db;

        public AvailabilityController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }

        // GET: Availability
        [AuthorizeRole(Feature = Features.Availability.ViewUnpublished)]
        public ViewResult Index(FilterArgs args)
        {
            // if one of the params is set, both are required
            if ((args.Year.HasValue && !args.Month.HasValue) || (!args.Year.HasValue && args.Month.HasValue))
            {
                ModelState.AddModelError("", "Both the year and month must be set to a value");
                
                return View(new WebApp.ViewModels.AvailabilityViewModels.IndexViewModel
                {
                    Months = new List<MonthGroup>(),
                    FilterArgs = args
                });
            }

            var userContext = User.Identity.GetUserContext();

            var availableDays = db.AvailableDays
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
                var thisMonth = new DateTime(now.Year, now.Month, 1);
                availableDays = availableDays
                    .Where(c => c.Day >= thisMonth);
            }

            var dto = availableDays.Select(AvailableDayDto.FromAvailableDayEntity.Expand()).ToList();

            var viewModel = dto.AsQueryable().Select(AvailableDayViewModel.FromAvailableDayDto.Expand());

            var months = viewModel
                .GroupBy(ad => new { Month = new DateTime(ad.Day.Year, ad.Day.Month, 1) })
                .Select(ad => new MonthGroup
                {
                    Month = ad.Key.Month,
                    AvailableDays = ad
                }).ToList();

            var model = new WebApp.ViewModels.AvailabilityViewModels.IndexViewModel()
            {
                Months = months,
                Calendar = CultureInfo.CurrentCulture.Calendar,
                Today = now,
                FilterArgs = args
            };

            return View(model);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ViewResult> AddDay(Guid id)
        {
            var physician = db.AspNetUsers.Single(c => c.Id == id);

            var availableDays = await db.AvailableDays.Where(c => c.PhysicianId == id).ToListAsync();

            var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray<string>();
            ViewBag.AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));
            ViewBag.PhysicianName = physician.GetDisplayName();
            var model = new AvailableDay()
            {
                PhysicianId = id,
                Day = now
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
                    db.AvailableDays.Add(day);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { PhysicianId = model.PhysicianId });
            }
            return View(model);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<RedirectResult> AddSlots(short AvailableDayId, short StartHour, short StartMinute, short Duration, byte Repeat)
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
                db.AvailableSlots.Add(slot);
                await db.SaveChangesAsync();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<RedirectResult> CancelDay(int id)
        {
            var day = await db.AvailableDays.FirstAsync(c => c.Id == id);

            if (day.AvailableSlots.Any(a => a.ServiceRequests.Any()))
            {
                ModelState.AddModelError("", "Slots have already been booked.");
            }
            db.AvailableDays.Remove(day);
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<JsonResult> CancelSlot(int availableSlotId)
        {
            var slot = await db.AvailableSlots.FirstAsync(c => c.Id == availableSlotId);

            if (slot.ServiceRequests.Any())
            {
                ModelState.AddModelError("", "Slot has already been booked.");
            }
            db.AvailableSlots.Remove(slot);
            await db.SaveChangesAsync();
            return Json(new { availableDayId = slot.AvailableDayId });
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public ActionResult ShowNewAvailableDayResourceForm(short availableDayId)
        {
            var form = new AvailableDayResourceForm();
            form.AvailableDayId = availableDayId;
            
            return PartialView("~/Views/Availability/_AvailableDayResourceForm.cshtml", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> SaveAvailableDayResourceForm(AvailableDayResourceForm form)
        {
            if (ModelState.IsValid)
            {
                var item = new AvailableDayResource();
                item.Id = Guid.NewGuid();
                item.AvailableDayId = form.AvailableDayId;
                item.UserId = form.UserId;
                item.CreatedUser = User.Identity.GetGuidUserId().ToString();
                item.CreatedDate = SystemTime.Now();
                item.ModifiedUser = User.Identity.GetGuidUserId().ToString();
                item.ModifiedDate = SystemTime.Now();

                db.AvailableDayResources.Add(item);
                await db.SaveChangesAsync();
                return Json(item);
            }

            return PartialView("~/Views/Availability/_AvailableDayResourceForm.cshtml", form);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> ShowDeleteAvailableDayResourceForm(Guid resourceId)
        {
            return PartialView("_DeleteAvailableDayResourceModalForm", resourceId);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> DeleteAvailableDayResource(Guid resourceId)
        {
            var entity = await db.AvailableDayResources.FindAsync(resourceId);
            var availableDayId = entity.AvailableDayId;

            db.AvailableDayResources.Remove(entity);
            await db.SaveChangesAsync();

            return Json(new
            {
                availableDayId = availableDayId
            });
        }

        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> AvailableDayResourceList(short availableDayId)
        {
            var dto = db.AvailableDayResources
                .Where(adr => adr.AvailableDayId == availableDayId)
                .Select(AvailableDayResourceDto.FromAvailableDayResourceEntity.Expand())
                .ToList();

            var viewModel = dto.AsQueryable().Select(AvailableDayResourceViewModel.FromAvailableDayResourceDto.Expand());

            return PartialView("~/Views/Availability/_AvailableDayResourceList.cshtml", viewModel);
        }


        public async Task<ActionResult> GetSlotsByAvailableDay(DateTime day)
        {
            var ad = await db.AvailableDays.Include(a => a.AvailableSlots)
                .SingleOrDefaultAsync(c => c.PhysicianId == physicianId && c.Day == day);

            if (ad == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Not available this day.");
            }

            return Json(
                new
                {
                    Day = ad.Day,
                    IsPrebook = ad.IsPrebook,
                    CompanyId = ad.CompanyId,
                    CompanyName = ad.Company == null ? string.Empty : ad.Company.Name,
                    LocationId = ad.LocationId,
                    LocationName = ad.Address == null ? string.Empty : ad.Address.Name,
                    LocationOwner = ad.Address == null ? null : ad.Address.OwnerGuid,
                    Slots = ad.AvailableSlots
                        .OrderBy(s => s.StartTime)
                        .Select(s => new
                        {
                            Id = s.Id,
                            StartTime = s.StartTime.ToShortTimeSafe(),
                            Duration = s.Duration,
                            Title = (s.ServiceRequests.Where(sr => !sr.CancelledDate.HasValue).Any() ? s.ServiceRequests.Where(sr => !sr.CancelledDate.HasValue).FirstOrDefault().ClaimantName + " - " + s.ServiceRequests.FirstOrDefault().Id.ToString() : string.Empty),
                            IsAvailable = !s.ServiceRequests.Where(sr => !sr.CancelledDate.HasValue).Any()
                        })
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> ShowAvailableDayCompanyForm(short availableDayId)
        {
            var ad = await db.AvailableDays.FindAsync(availableDayId);
            var form = new AvailableDayCompanyForm { AvailableDayId = availableDayId, CompanyId = ad.CompanyId };
            return PartialView("_AvailableDayCompanyModalForm", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> SaveAvailableDayCompanyForm(AvailableDayCompanyForm form)
        {
            if (ModelState.IsValid)
            {
                var ad = await db.AvailableDays.FindAsync(form.AvailableDayId);
                ad.CompanyId = form.CompanyId;
                ad.ModifiedDate = now;
                ad.ModifiedUser = loggedInUserId.ToString();

                await db.SaveChangesAsync();

                return Json(new
                {
                    availableDayId = form.AvailableDayId
                });
            }
            return PartialView("_AvailableDayCompanyModalForm", form);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> ShowAvailableDayAddressForm(short availableDayId)
        {
            var ad = await db.AvailableDays.FindAsync(availableDayId);
            var form = new AvailableDayAddressForm { AvailableDayId = availableDayId, AddressId = ad.LocationId, PhysicianId = ad.PhysicianId };
            return PartialView("_AvailableDayAddressModalForm", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> SaveAvailableDayAddressForm(AvailableDayAddressForm form)
        {
            if (ModelState.IsValid)
            {
                var ad = await db.AvailableDays.FindAsync(form.AvailableDayId);
                ad.LocationId = form.AddressId;
                ad.ModifiedDate = now;
                ad.ModifiedUser = loggedInUserId.ToString();

                await db.SaveChangesAsync();

                return Json(new
                {
                    availableDayId = form.AvailableDayId
                });
            }
            return PartialView("_AvailableDayAddressModalForm", form);
        }
    }
}