﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orvosi.Shared.Model;
using System.Net;
using WebApp.Library.Extensions;
using Orvosi.Shared.Enums;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Library.Filters;
using WebApp.ViewModels;
using Orvosi.Data.Filters;
using WebApp.Models;
using LinqKit;
using Orvosi.Data;
using WebApp.Library;
using System.Security.Principal;

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
        public ActionResult Discussion(int serviceRequestId)
        {
            var dto = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestDto.FromEntityForMessages.Expand())
                .Single();

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(dto);
            return PartialView("_Discussion", viewModel);
        }

        public ActionResult PostMessage(int serviceRequestId, string message)
        {
            var newMessage = new Orvosi.Data.ServiceRequestMessage()
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

        public ActionResult GetMessage(Guid serviceRequestMessageId)
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