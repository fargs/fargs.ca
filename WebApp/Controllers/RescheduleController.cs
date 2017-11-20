using FluentValidation.Mvc;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library;

namespace WebApp.Controllers
{
    public class RescheduleController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;
        private ViewDataService viewDataService;

        public RescheduleController(OrvosiDbContext db, WorkService service, ViewDataService viewDataService, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
            this.viewDataService = viewDataService;
        }

        [HttpGet]
        public async Task<PartialViewResult> ShowRescheduleForm(int serviceRequestTaskId)
        {
            var task = await db.ServiceRequestTasks.FindAsync(serviceRequestTaskId);
            var dto = task.ServiceRequest;
            if (!dto.AppointmentDate.HasValue)
            {
                throw new Exception("Service request does not have an appointment date to reschedule.");
            }
            var form = new RescheduleForm();
            form.ServiceRequestTaskId = task.Id;
            form.ServiceRequestId = dto.Id;
            form.PhysicianId = dto.PhysicianId;
            form.AppointmentDate = dto.AppointmentDate.Value;
            form.OriginalAvailableSlotId = dto.AvailableSlotId;
            form.AvailableSlotId = dto.AvailableSlotId;
            form.AddressId = dto.AddressId;

            return PartialView("_RescheduleModalForm", form);
        }

        [HttpGet]
        public PartialViewResult SelectedDayChanged(RescheduleForm form)
        {
            form.AvailableSlotId = null;
            return PartialView("_RescheduleModalForm", form);
        }

        public PartialViewResult SlotPicker(RescheduleForm form)
        {
            var selectList = this.viewDataService.GetPhysicianAvailableSlotSelectList(form.PhysicianId, form.AppointmentDate, form.OriginalAvailableSlotId);
            return PartialView(selectList);
        }

        [HttpPost]
        public async Task<ActionResult> Reschedule(RescheduleForm form)
        {
            if (ModelState.IsValid)
            {
                //var response = await service.Reschedule(form);
                //response.AddToModelState(ModelState, null);
                await service.Reschedule(form);
                return Json(new
                {
                    serviceRequestTaskId = form.ServiceRequestTaskId,
                    serviceRequestId = form.ServiceRequestId
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("_RescheduleModalForm", form);
        }

        public DateTime[] GetUnavailableDays(Guid physicianId, DateTime day)
        {
            var unavailableDates = new List<DateTime>
            {
                new DateTime(2016, 09, 22)
            };
            return unavailableDates.ToArray();
        }
    }
}