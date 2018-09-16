using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library;
using WebApp.Library.Helpers;
using WebApp.Models;
using WebApp.ViewModels;
using Orvosi.Data.Filters;
namespace WebApp.Controllers
{
    public class TeleconferenceController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;
        private IEmailService emailService;

        public TeleconferenceController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal, IEmailService emailService) : base(now, principal)
        {
            this.db = db;
            this.service = service;
            this.emailService = emailService;
        }

        public PartialViewResult ListByDay(DateTime day)
        {
            var query = db.Teleconferences
                .Where(c => c.AppointmentDate == day);

            query = query.CanAccess(loggedInUserId, physicianId, loggedInRoleId);

            var dto = query.Select(TeleconferenceDto.FromEntityForDaySheet.Expand()).ToList();

            var teleconferenceViewModels = dto.AsQueryable().Select(TeleconferenceViewModel.FromTeleconferenceDtoForDaySheet.Expand());

            var viewModel = new TeleconferenceDayListViewModel()
            {
                Day = day,
                Teleconferences = teleconferenceViewModels
            };

            return PartialView("_ListByDay", viewModel);
        }

        [HttpGet]
        public PartialViewResult List(int serviceRequestId)
        {
            var dto = db.Teleconferences
                .Where(c => c.ServiceRequestId == serviceRequestId)
                .Select(TeleconferenceDto.FromEntity.Expand())
                .ToList();

            var teleconferenceViewModels = dto.AsQueryable().Select(TeleconferenceViewModel.FromTeleconferenceDto.Expand());

            var commentsDto = db.ServiceRequestComments
                .Where(c => c.ServiceRequestId == serviceRequestId & c.CommentTypeId == CommentTypes.Teleconference)
                .Select(CommentDto.FromServiceRequestCommentEntity.Expand())
                .ToList();

            var comments = commentsDto.AsQueryable()
                .Select(CommentViewModel.FromCommentDto.Expand());

            var viewModel = new TeleconferenceListViewModel()
            {
                ServiceRequestId = serviceRequestId,
                Teleconferences = teleconferenceViewModels,
                Notes = comments
            };

            return PartialView("_List", viewModel);
        }


        [HttpGet]
        public ActionResult ShowTeleconferenceForm(int serviceRequestId, Guid? teleconferenceId)
        {

            TeleconferenceForm viewModel;
            if (teleconferenceId.HasValue)
            {
                var dto = TeleconferenceDto.FromEntity.Invoke(db.Teleconferences.Find(teleconferenceId));

                viewModel = new TeleconferenceForm()
                {
                    TeleconferenceId = dto.Id,
                    ServiceRequestId = serviceRequestId,
                    AppointmentDate = dto.AppointmentDate,
                    StartTime = dto.StartTime
                };
            }
            else
            {
                viewModel = new TeleconferenceForm()
                {
                    ServiceRequestId = serviceRequestId,
                    AppointmentDate = now
                };
            }

            return PartialView("_TeleconferenceModalForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveTeleconference(TeleconferenceForm form)
        {
            if (ModelState.IsValid)
            {
                var teleconferenceId = await service.SaveTeleconference(form);
                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId,
                    teleconferenceId = teleconferenceId

                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("_TeleconferenceModalForm", form);
        }


        [HttpGet]
        public ActionResult ShowTeleconferenceResultForm(Guid teleconferenceId)
        {

            // database to dto
            var teleconference = db.Teleconferences
                .Where(c => c.Id == teleconferenceId)
                .Select(TeleconferenceDto.FromEntity.Expand())
                .Single();

            var sr = db.ServiceRequests
                .Where(s => s.Id == teleconference.ServiceRequestId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForCaseNotification.Expand())
                .Single();

            var admin = db.ServiceRequestResources
                .Where(r => r.ServiceRequestId == sr.Id && r.RoleId == AspNetRoles.CaseCoordinator)
                .SingleOrDefault();

            // dto to viewmodel
            var caseNotificationViewModel = CaseNotificationViewModel.FromServiceRequestDto.Invoke(sr);
            var teleconferenceViewModel = TeleconferenceViewModel.FromTeleconferenceDto.Invoke(teleconference);

            var viewModel = new TeleconferenceNotificationViewModel()
            {
                CaseNotificationViewModel = caseNotificationViewModel,
                Teleconference = teleconferenceViewModel
            };

            // viewmodel to email
            var notificationViewModel = new WebApp.Views.Teleconference.NotificationViewModel
            {
                PhysicianName = sr.Physician.DisplayName,
                AdminEmail = admin == null ? string.Empty : admin.AspNetUser.Email,
                ClaimantName = sr.ClaimantName,
                ResultTypeId = teleconference.ResultTypeId
            };
            var body = HtmlHelpers.RenderPartialViewToString(this, "_Notification", notificationViewModel);

            var physicianEmail = (sr.Physician as ContactDto).Email;

            var message = new MailMessage();
            message.From = new MailAddress(physicianEmail);
            message.IsBodyHtml = true;

            // send email to the Reporting email for the company
            var company = db.Companies.Find(sr.Company.Id);
            if (!string.IsNullOrEmpty(company.ReportsEmail))
            {
                message.To.Add(company.ReportsEmail);
            }

            // always CC the admin if there is one
            if (admin != null)
            {
                message.CC.Add(admin.AspNetUser.Email);
            }

            message.Subject = "Teleconference regarding claimant " + sr.ClaimantName;
            message.Body = body;

            viewModel.Message = message;

            return PartialView("_TeleconferenceResultModalForm", viewModel);
        }

        [HttpGet]
        public ActionResult GetNotificationTemplate(byte? resultTypeId, int serviceRequestId)
        {
            // database to dto

            var sr = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForTeleconferenceNotification.Expand())
                .Single();

            var admin = db.ServiceRequestResources
                .Where(r => r.ServiceRequestId == sr.Id && r.RoleId == AspNetRoles.CaseCoordinator)
                .SingleOrDefault();

            var viewModel = new WebApp.Views.Teleconference.NotificationViewModel
            {
                PhysicianName = sr.Physician.DisplayName,
                AdminEmail = admin == null ? string.Empty : admin.AspNetUser.Email,
                ClaimantName = sr.ClaimantName,
                ResultTypeId = resultTypeId
            };

            return PartialView("_Notification", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ShowDeleteTeleconferenceForm(Guid teleconferenceId)
        {
            var dto = await db.Teleconferences.FindAsync(teleconferenceId);
            var viewModel = new TeleconferenceForm()
            {
                TeleconferenceId = dto.Id,
                ServiceRequestId = dto.ServiceRequestId,
                AppointmentDate = dto.AppointmentDate,
                StartTime = dto.StartTime
            };

            return PartialView("_DeleteTeleconferenceModalForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTeleconference(Guid teleconferenceId)
        {
            var teleconference = await db.Teleconferences.FindAsync(teleconferenceId);
            await service.DeleteTeleconference(teleconferenceId);
            return Json(new
            {
                serviceRequestId = teleconference.ServiceRequestId,
                teleconferenceId
            });
        }

        [HttpPost]
        public async Task<ActionResult> SendResult()
        {
            var message = new MailMessage();
            message.From = new MailAddress(Request.Form["Message.From"]);
            message.To.Add(Request.Form["Message.To"]);

            var cc = Request.Form["Message.CC"];
            if (!string.IsNullOrEmpty(cc))
                message.CC.Add(cc);

            message.Subject = Request.Form["Message.Subject"];
            message.Body = Request.Form["Message.Body"];

            var teleconferenceId = Guid.Parse(Request.Form["TeleconferenceId"]);
            var teleconference = await db.Teleconferences.FindAsync(teleconferenceId);
            var physician = await db.Physicians.FindAsync(teleconference.ServiceRequest.PhysicianId);

            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file.ContentLength != 0)
                {
                    var attachment = new Attachment(file.InputStream, file.FileName);
                    message.Attachments.Add(attachment);
                }
            }

            // send the email
            await emailService.SendEmailAsync(message);

            // update the status of the teleconference
            byte teleconferenceResultId;
            if (byte.TryParse(Request.Form["TeleconferenceResultId"], out teleconferenceResultId)) teleconference.TeleconferenceResultId = teleconferenceResultId;

            teleconference.TeleconferenceResultSentDate = now;

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}