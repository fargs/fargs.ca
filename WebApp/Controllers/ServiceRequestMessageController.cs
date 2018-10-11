using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.Views.ServiceRequestMessage;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    [AuthorizeRole(Feature = Features.ServiceRequest.LiveChat)]
    public class ServiceRequestMessageController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;

        public ServiceRequestMessageController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
        }
        // GET: ServiceRequestMessage
        public PartialViewResult Discussion(int serviceRequestId)
        {
            var dto = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestDto.FromEntityForMessages.Expand())
                .Single();

            var viewModel = DiscussionViewModel.FromServiceRequestDto(dto);
            return PartialView("_Discussion", viewModel);
        }

        public JsonResult PostMessage(int serviceRequestId, string message)
        {
            var newMessage = new ServiceRequestMessage()
            {
                Id = Guid.NewGuid(),
                Message = message,
                UserId = loggedInUserId,
                PostedDate = now,
                ServiceRequestId = serviceRequestId
            };
            db.ServiceRequestMessages.Add(newMessage);
            db.SaveChanges();
            return Json(newMessage);
        }

        public PartialViewResult GetMessage(Guid serviceRequestMessageId)
        {
            var dto = db.ServiceRequestMessages
                .Where(srm => srm.Id == serviceRequestMessageId)
                .Select(MessageDto.FromServiceRequestMessageEntity.Expand())
                .Single();

            var viewModel = MessageViewModel.FromMessageDto.Invoke(dto);
            return PartialView("_ServiceRequestMessage", viewModel);
        }
    }
}