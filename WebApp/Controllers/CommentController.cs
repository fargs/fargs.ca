using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
namespace WebApp.Controllers
{
    public class CommentController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;
        private IEmailService emailService;

        public CommentController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal, IEmailService emailService) : base(now, principal)
        {
            this.db = db;
            this.service = service;
            this.emailService = emailService;
        }

        [HttpGet]
        public PartialViewResult List(int serviceRequestId)
        {            
            var dto = db.ServiceRequestComments
                .CanAccess(loggedInUserId) //this filter is duplicated in the ServiceRequestDto.FromServiceRequestEntity projection
                .Where(c => c.ServiceRequestId == serviceRequestId)
                .Select(CommentDto.FromServiceRequestCommentEntity.Expand())
                .ToList();

            var commentViewModels = dto.AsQueryable().Select(CommentViewModel.FromCommentDto.Expand());
            
            var viewModel = new CommentListViewModel()
            {
                ServiceRequestId = serviceRequestId,
                Comments = commentViewModels
            };
            
            return PartialView("_List", viewModel);
        }

        [HttpGet]
        public ActionResult ShowCommentForm(int serviceRequestId, Guid? commentId, byte? commentTypeId)
        {

            CommentForm viewModel;
            if (commentId.HasValue)
            {
                var dto = CommentDto.ForCommentForm.Invoke(db.ServiceRequestComments.Find(commentId));

                viewModel = new CommentForm()
                {
                    CommentId = dto.Id,
                    ServiceRequestId = serviceRequestId,
                    Message = dto.Message,
                    CommentTypeId = dto.CommentTypeId,
                    IsPrivate = dto.IsPrivate,
                    Physician = ContactViewModel<Guid>.FromContactDto.Invoke(dto.Physician),
                    AccessList = dto.AccessList.Select(a => a.Id).ToList()
                };
            }
            else
            {
                var sr = db.ServiceRequests.Find(serviceRequestId);
                var contact = ContactDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser);
                viewModel = new CommentForm()
                {
                    ServiceRequestId = serviceRequestId,
                    CommentTypeId = commentTypeId.HasValue ? commentTypeId.Value : CommentTypes.Note,
                    Physician = ContactViewModel<Guid>.FromContactDto.Invoke(contact),
                    IsPrivate = true
                };

                var viewDataService = new WebApp.Library.ViewDataService(db, User);
                var teamMembers = viewDataService.GetCaseResourceSelectList(serviceRequestId);
                // default to grant access to everyone on the case
                foreach (var member in teamMembers)
                {
                    viewModel.AccessList.Add(Guid.Parse(member.Value));
                }
            }

            return PartialView("_CommentModalForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveComment(CommentForm form)
        {
            if (ModelState.IsValid)
            {
                var commentId = await service.SaveComment(form);
                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId,
                    commentId = commentId

                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("_CommentModalForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> ShowDeleteCommentForm(Guid commentId)
        {
            var dto = await db.ServiceRequestComments.FindAsync(commentId);
            var viewModel = new CommentForm()
            {
                CommentId = dto.Id,
                ServiceRequestId = dto.ServiceRequestId,
                Message = dto.Comment,
                CommentTypeId = dto.CommentTypeId,
                IsPrivate = dto.IsPrivate
            };

            return PartialView("_DeleteCommentModalForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(Guid commentId)
        {
            var comment = await db.ServiceRequestComments.FindAsync(commentId);
            await service.DeleteComment(commentId);
            return Json(new
            {
                serviceRequestId = comment.ServiceRequestId,
                commentId = commentId
            });
        }

        [HttpPost]
        public async Task<ActionResult> NotifyTeam(Guid commentId)
        {
            // database to dto
            var comment = db.ServiceRequestComments
                .Where(c => c.Id == commentId)
                .Select(CommentDto.FromServiceRequestCommentEntity.Expand())
                .Single();

            var sr = db.ServiceRequests
                .Where(s => s.Id == comment.ServiceRequestId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForCaseNotification.Expand())
                .Single();

            // dto to viewmodel
            var caseNotificationViewModel = CaseNotificationViewModel.FromServiceRequestDto.Invoke(sr);
            var commentViewModel = CommentViewModel.FromCommentDto.Invoke(comment);

            var viewModel = new CommentNotificationViewModel()
            {
                CaseNotificationViewModel = caseNotificationViewModel,
                Comment = commentViewModel
            };

            // viewmodel to email
            var body = HtmlHelpers.RenderPartialViewToString(this, "_Notification", viewModel);
            
            var message = new MailMessage();
            message.From = new MailAddress(comment.PostedBy.Email);
            message.IsBodyHtml = true;

            // Always add the physician, except when the physician posted the comment.
            var physicianEmail = (sr.Physician as ContactDto).Email;
            if (physicianEmail != comment.PostedBy.Email)
                message.To.Add(physicianEmail);
            foreach (ContactDto access in comment.AccessList)
            {
                message.To.Add(access.Email);
            }

            message.Subject = "Note from " + viewModel.Comment.PostedBy.Name + " (case #" + viewModel.CaseNotificationViewModel.ServiceRequestId.ToString() + ")";
            message.Body = body;

            // send the message
            await emailService.SendEmailAsync(message);

            return Json(new
            {
                success = true
            });
        }
    }
}