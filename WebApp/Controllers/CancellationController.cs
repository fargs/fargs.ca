using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.ViewModels;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class CancellationController : BaseController
    {
        private OrvosiDbContext db;
        private WorkService service;

        public CancellationController(OrvosiDbContext db, WorkService service, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
        }
        [HttpGet]
        public PartialViewResult ShowCancelRequest(int serviceRequestId)
        {
            // retrieve the data from the database
            var serviceRequest = db.ServiceRequests.WithId(serviceRequestId).Select(ServiceRequestDto.FromEntityForCancellationForm.Expand()).Single();

            var policy = db.GetServiceCatalogueRate(serviceRequest.PhysicianId, serviceRequest.CompanyGuid).Single();

            var policyResult = serviceRequest.IsLateCancellationPolicyViolated(policy.LateCancellationPolicy, now);

            var viewModel = new CancellationForm()
            {
                AppointmentDate = serviceRequest.AppointmentDate,
                ServiceRequestId = serviceRequestId,
                CancelledDate = now,
                LateCancellationPolicy = policy.LateCancellationPolicy,
                IsLateCancellationPolicyViolated = policyResult
            };

            return PartialView("CancellationModalForm", viewModel);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.Cancel)]
        public async Task<ActionResult> LateCancellationPolicyViolation(int serviceRequestId, DateTime cancelledDate)
        {
            // retrieve the data from the database
            var serviceRequest = db.ServiceRequests.WithId(serviceRequestId).Select(ServiceRequestDto.FromEntityForCancellationForm.Expand()).Single();

            var policy = db.GetServiceCatalogueRate(serviceRequest.PhysicianId, serviceRequest.CompanyGuid).Single();

            var policyResult = serviceRequest.IsLateCancellationPolicyViolated(policy.LateCancellationPolicy, cancelledDate); // Cancelled Date is the difference between ShowCancelRequest

            return PartialView("LateCancellationPolicyViolation", policyResult);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.Cancel)]
        public async Task<ActionResult> CancelRequest(CancellationForm form)
        {
            if (ModelState.IsValid)
            {
                await service.CancelRequest(form);

                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("CancellationModalForm", form);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.Cancel)]
        public async Task<JsonResult> CancelRequestUndo(int serviceRequestId)
        {
            await service.CancelRequestUndo(serviceRequestId);
            return Json(new
            {
                serviceRequestId = serviceRequestId
            });
        }

        [HttpGet]
        public PartialViewResult ShowDeleteRequest(int serviceRequestId)
        {
            var model = db.ServiceRequests
                .WithId(serviceRequestId)
                .Select(ServiceRequestDto.FromServiceRequestEntityForCase.Expand())
                .SingleOrDefault();

            var viewModel = CaseViewModel.FromServiceRequestDto.Invoke(model);

            return PartialView("DeleteModalForm", viewModel);
        }

        [AuthorizeRole(Feature = Features.ServiceRequest.Delete)]
        public async Task<ActionResult> DeleteRequest(int? serviceRequestId)
        {
            var serviceRequest = await db.ServiceRequests.FindAsync(serviceRequestId);

            if (!serviceRequest.CancelledDate.HasValue)
            {
                this.ModelState.AddModelError("", "Service Request must be cancelled before being deleted.");
                return View("Delete");
            }

            if (ModelState.IsValid)
            {
                await service.DeleteRequest(serviceRequest);

                return Json(new
                {
                    serviceRequestId = serviceRequest.Id
                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("DeleteModalForm", serviceRequest.Id);
        }
    }
}