using LinqKit;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.ViewModels;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class CancellationController : BaseController
    {
        [HttpGet]
        public ActionResult ShowCancelRequest(int serviceRequestId)
        {
            var viewModel = new CancellationForm()
            {
                ServiceRequestId = serviceRequestId,
                CancelledDate = now
            };

            return PartialView("CancellationModalForm", viewModel);
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
        public async Task<ActionResult> CancelRequestUndo(int serviceRequestId)
        {
            await service.CancelRequestUndo(serviceRequestId);
            return Json(new
            {
                serviceRequestId = serviceRequestId
            });
        }

        [HttpGet]
        public ActionResult ShowDeleteRequest(int serviceRequestId)
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