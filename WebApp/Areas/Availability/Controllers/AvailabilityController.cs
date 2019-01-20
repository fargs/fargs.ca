using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
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
using Features = ImeHub.Models.Enums.Features.PhysicianPortal;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Availability.Controllers
{
    public class AvailabilityController : BaseController
    {
        private ImeHubDbContext db;
        private DateTime _selectedDate;

        public AvailabilityController(ImeHubDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
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
                    var day = new AvailableDay()
                    {
                        Id = Guid.NewGuid(),
                        Day = DateTime.Parse(item),
                        PhysicianId = form.PhysicianId.Value,
                        CompanyId = form.CompanyId,
                        AddressId = form.AddressId
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
        public PartialViewResult ShowAddSlotsForm(Guid availableDayId)
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
                var slot = new AvailableSlot()
                {
                    Id = Guid.NewGuid(),
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
        public async Task<RedirectResult> CancelDay(Guid id)
        {
            var day = await db.AvailableDays.FirstAsync(c => c.Id == id);

            //if (day.AvailableSlots.Any(a => a.ServiceRequests.Any()))
            //{
            //    ModelState.AddModelError("", "Slots have already been booked.");
            //}
            db.AvailableDays.Remove(day);
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<JsonResult> CancelSlot(Guid availableSlotId)
        {
            var slot = await db.AvailableSlots.FirstAsync(c => c.Id == availableSlotId);

            //if (slot.ServiceRequests.Any())
            //{
            //    ModelState.AddModelError("", "Slot has already been booked.");
            //}
            db.AvailableSlots.Remove(slot);
            await db.SaveChangesAsync();
            return Json(new { availableDayId = slot.AvailableDayId });
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public ActionResult ShowNewAvailableDayResourceForm(Guid availableDayId)
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
                
                db.AvailableDayResources.Add(item);
                await db.SaveChangesAsync();
                return Json(item);
            }

            return PartialView("_AvailableDayResourceForm", form);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public ActionResult ShowDeleteAvailableDayResourceForm(Guid resourceId)
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
        public async Task<ActionResult> AvailableDayResourceList(Guid availableDayId)
        {
            var dto = db.AvailableDayResources
                .AsNoTracking()
                .AsExpandable()
                .Where(adr => adr.AvailableDayId == availableDayId)
                .Select(AvailableDayResourceModel.FromAvailableDayResource)
                .ToList();

            var viewModel = dto.AsQueryable().Select(AvailableDayResourceViewModel.FromAvailableDayResourceModel);

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
                    CompanyId = ad.CompanyId,
                    CompanyName = ad.Company == null ? string.Empty : ad.Company.Name,
                    AddressId = ad.AddressId,
                    AddressName = ad.Address == null ? string.Empty : ad.Address.Name,
                    AddressOwner = ad.Address == null ? null : (ad.Address.PhysicianId ?? ad.Address.CompanyId),
                    Slots = ad.AvailableSlots
                        .OrderBy(s => s.StartTime)
                        .Select(s => new
                        {
                            Id = s.Id,
                            StartTime = s.StartTime.ToShortTimeSafe(),
                            Duration = s.Duration
                            //Title = (s.ServiceRequests.Where(sr => !sr.CancelledDate.HasValue).Any() ? s.ServiceRequests.Where(sr => !sr.CancelledDate.HasValue).FirstOrDefault().ClaimantName + " - " + s.ServiceRequests.FirstOrDefault().Id.ToString() : string.Empty),
                            //IsAvailable = !s.ServiceRequests.Where(sr => !sr.CancelledDate.HasValue).Any()
                        })
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> ShowAvailableDayCompanyForm(Guid availableDayId)
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
        public async Task<ActionResult> ShowAvailableDayAddressForm(Guid availableDayId)
        {
            var ad = await db.AvailableDays.FindAsync(availableDayId);
            var form = new AvailableDayAddressForm(db, identity, now) { AvailableDayId = availableDayId, AddressId = ad.AddressId, PhysicianId = ad.PhysicianId };
            return PartialView("_AvailableDayAddressModalForm", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.Manage)]
        public async Task<ActionResult> SaveAvailableDayAddressForm(AvailableDayAddressForm form)
        {
            if (ModelState.IsValid)
            {
                var ad = await db.AvailableDays.FindAsync(form.AvailableDayId);
                ad.AddressId = form.AddressId;
                
                await db.SaveChangesAsync();

                return Json(new
                {
                    availableDayId = form.AvailableDayId
                });
            }
            return PartialView("_AvailableDayAddressModalForm", form);
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public async Task<ActionResult> ShowBookingForm(Guid availableSlotId)
        {
            var form = new BookingForm(availableSlotId, db, identity, now);

            return PartialView("Booking", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Availability.BookAssessment)]
        public async Task<ActionResult> BookAssessment(BookingForm form)
        {
            if (!ModelState.IsValid)
            {
                form.ViewData = new BookingForm.ViewDataModel(form.AvailableSlotId, db, physicianId.Value, form.CompanyId, now);
                return PartialView("~/Views/ServiceRequest/Booking.cshtml", form);
            }

            var id = await BookAppointment(form);
            return Json(new
            {
                serviceRequestId = id
            });

        }

        public async Task<ActionResult> BookAppointment(BookingForm form)
        {
            var userId = loggedInUserId;
            
            var slot = await db.AvailableSlots.FindAsync(form.AvailableSlotId);

            var sr = new ServiceRequest();
            sr.Id = Guid.NewGuid();
            sr.CaseNumber = db.Cases.GetNextCaseNumber(physicianId.Value);
            sr.StatusId = (byte)Enums.ServiceRequestStatus.Active;
            sr.StatusChangedById = userId;
            sr.StatusChangedDate = now;
            sr.RequestedDate = now;
            sr.ServiceId = form.ServiceId;
            sr.PhysicianId = physicianId.Value;
            sr.AddressId = form.AddressId;
            sr.AppointmentDate = form.AppointmentDate;
            sr.AvailableSlotId = form.AvailableSlotId;
            sr.StartTime = slot.StartTime;
            sr.EndTime = slot.EndTime;
            sr.DueDate = form.DueDate;
            sr.AlternateKey = form.CompanyReferenceId;
            sr.ClaimantName = form.ClaimantName;
            sr.ReferralSource = form.ReferralSource;
            sr.CancellationStatusId = (byte)Enums.CancellationStatus.NotCancelled;
            sr.CancellationStatusChangedById = userId;
            sr.CancellationStatusChangedDate = now;

            //// clone the workflow template tasks
            //var requestTemplate = await db.Workflows.FindAsync(sr.WorkflowId);
            //foreach (var template in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted().Select(ServiceRequestTemplateTaskDto.FromEntity.Expand()))
            //{
            //    var st = new Orvosi.Data.ServiceRequestTask();
            //    st.Guidance = null;
            //    st.ObjectGuid = Guid.NewGuid();
            //    st.ResponsibleRoleId = template.ResponsibleRoleId;
            //    st.Sequence = template.Sequence;
            //    st.ShortName = template.ShortName;
            //    st.TaskId = template.TaskId;
            //    st.TaskName = template.TaskName;
            //    st.ModifiedDate = now;
            //    st.ModifiedUser = userId.ToString();
            //    st.CreatedDate = now;
            //    st.CreatedUser = userId.ToString();
            //    // Assign tasks to physician and case coordinator to start
            //    st.AssignedTo = (template.ResponsibleRoleId == AspNetRoles.CaseCoordinator ? sr.CaseCoordinatorId : (template.ResponsibleRoleId == AspNetRoles.Physician ? sr.PhysicianId as Nullable<Guid> : null));
            //    st.ServiceRequestTemplateTaskId = template.Id;
            //    st.TaskType = template.DueDateType;
            //    st.Workload = null;
            //    st.DueDateDurationFromBaseline = template.DueDateDurationFromBaseline;
            //    st.DueDate = GetTaskDueDate(sr.AppointmentDate, sr.DueDate, template);
            //    st.TaskStatusId = TaskStatuses.ToDo;
            //    st.TaskStatusChangedBy = userId;
            //    st.TaskStatusChangedDate = now;
            //    st.IsCriticalPath = template.IsCriticalPath;
            //    st.EffectiveDateDurationFromBaseline = template.EffectiveDateDurationFromBaseline;
            //    st.EffectiveDate = GetEffectiveDate(sr.AppointmentDate, template);

            //    sr.ServiceRequestTasks.Add(st);
            //}

            //sr.UpdateIsClosed();
            db.ServiceRequests.Add(sr);

            //await db.SaveChangesAsync();

            //// Clone the task dependencies
            //foreach (var taskTemplate in requestTemplate.ServiceRequestTemplateTasks.AsQueryable().AreNotDeleted())
            //{
            //    foreach (var dependentTemplate in taskTemplate.Child)
            //    {
            //        var task = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == taskTemplate.Id);
            //        var dependent = sr.ServiceRequestTasks.First(srt => srt.ServiceRequestTemplateTaskId == dependentTemplate.Id);
            //        task.Child.Add(dependent);
            //    }
            //}

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }

            //await UpdateDependentTaskStatuses(sr.Id);
            //await UpdateServiceRequestStatus(sr.Id);

            return Json(sr.Id);
        }

    }
}