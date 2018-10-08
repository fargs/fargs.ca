using LinqKit;
using Orvosi.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Areas.Availability.Views.Home;
using WebApp.Areas.Shared;
using WebApp.Library.Extensions;
using WebApp.Library.Filters;
using WebApp.Models;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Areas.Availability.Controllers
{
    public class AvailabilityController : BaseController
    {
        private OrvosiDbContext db;
        private DateTime _selectedDate;

        public AvailabilityController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            if (!physicianId.HasValue)
                throw new PhysicianNullException();
            this.db = db;
        }

        [HttpGet]
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public PartialViewResult ShowAddDayForm(DateTime? selectedDate)
        {
            _selectedDate = selectedDate.GetValueOrDefault(now);
            var viewModel = new AddDayFormModel(_selectedDate, db, identity, now);

            return PartialView("AddDay", viewModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> SaveAddDayForm(AddDayFormModel form)
        {
            if (ModelState.IsValid)
            {
                var dates = Request.Form["AvailableDays"].Split(',');
                foreach (var item in dates)
                {
                    var day = new Orvosi.Data.AvailableDay()
                    {
                        Day = DateTime.Parse(item),
                        PhysicianId = form.PhysicianId.Value,
                        CompanyId = form.CompanyId,
                        LocationId = form.LocationId,
                        IsPrebook = form.IsPrebook
                    };
                    db.AvailableDays.Add(day);
                }
                await db.SaveChangesAsync();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("AddDay", form);
        }
        [HttpGet]
        [ChildActionOnlyOrAjax]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public PartialViewResult ShowAddSlotsForm(short availableDayId)
        {
            var viewModel = new AddSlotsFormModel();

            return PartialView("AddSlotsForm", viewModel);
        }
        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> SaveAddSlotsForm(AddSlotsFormModel form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return PartialView("AddSlotsForm", form);
            }
            var firstStartTime = new TimeSpan(form.StartHour, form.StartMinute, 0);
            for (int i = 0; i < form.Repeat; i++)
            {
                var startTime = firstStartTime.Add(new TimeSpan(0, form.Duration * i, 0));
                var endTime = startTime.Add(new TimeSpan(0, form.Duration, 0));
                var slot = new Orvosi.Data.AvailableSlot()
                {
                    AvailableDayId = form.AvailableDayId,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = form.Duration
                };
                db.AvailableSlots.Add(slot);
            }
            await db.SaveChangesAsync();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
            var form = new AvailableDayResourceForm(db, identity, now);
            form.AvailableDayId = availableDayId;

            return PartialView("_AvailableDayResourceForm", form);
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

            return PartialView("_AvailableDayResourceForm", form);
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

            return PartialView("_AvailableDayResourceList", viewModel);
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
            var form = new AvailableDayCompanyForm(db, identity, now) { AvailableDayId = availableDayId, CompanyId = ad.CompanyId };
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
            var form = new AvailableDayAddressForm(db, identity, now) { AvailableDayId = availableDayId, AddressId = ad.LocationId, PhysicianId = ad.PhysicianId };
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