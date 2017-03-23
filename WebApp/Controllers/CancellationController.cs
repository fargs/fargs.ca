using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library.Filters;
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
    }
}