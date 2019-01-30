using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.ServiceRequests.Views.Assessment;
using WebApp.Areas.Shared;
using WebApp.Library.Filters;
using Features = ImeHub.Models.Enums.Features.PhysicianPortal;

namespace WebApp.Areas.ServiceRequests.Controllers
{
    public class AssessmentController : BaseController
    {
        private ImeHubDbContext db;

        public AssessmentController(ImeHubDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public ViewResult Details(Guid id)
        {
            var assessment = db.ServiceRequests
                .AsExpandable()
                .Where(sr => sr.Id == id)
                //.CanAccess(loggedInUserId, physicianId, loggedInRoleId)
                .Select(ServiceRequestModel.FromServiceRequest)
                .SingleOrDefault();

            if (assessment == null)
            {
                return View("Unauthorized");
            }

            var viewModel = new DetailsViewModel(assessment, identity, now);

            return View(viewModel);
        }
    }
}