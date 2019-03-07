using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.ServiceRequests.Views.ServiceRequest;
using WebApp.Areas.Shared;
using System.Threading.Tasks;

namespace WebApp.Areas.ServiceRequests.Controllers
{
    public class ServiceRequestController : BaseController
    {
        private ImeHubDbContext db;
        private static readonly string SERVICEREQUESTFORM = "ServiceRequestForm";
        public ServiceRequestController(ImeHubDbContext db, IPrincipal principal, DateTime now) : base(now, principal)
        {
            this.db = db;
        }

        #region Create

        public ActionResult ShowForm()
        {
            var form = new ServiceRequestForm(physicianId.Value, db);
            return PartialView(SERVICEREQUESTFORM, form);
        }

        [HttpPost]
        public ActionResult RefreshForm(ServiceRequestForm form)
        {
            var formWithViewData = new ServiceRequestForm(form, db);

            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }

            return PartialView(SERVICEREQUESTFORM, formWithViewData);
        }

        [HttpPost]
        public ActionResult ValidateForm(ServiceRequestForm form)
        {
            var formWithViewData = new ServiceRequestForm(form, db);

            return PartialView(SERVICEREQUESTFORM, formWithViewData);
        }

        [HttpPost]
        public async Task<ActionResult> SaveFormAsync(ServiceRequestForm form)
        {
            var formWithViewData = new ServiceRequestForm(form, db);

            if (!ModelState.IsValid) return PartialView(SERVICEREQUESTFORM, formWithViewData);

            if (form.ServiceRequestId.HasValue)
            {
                // TODO: Get the request and add this form data to it
                throw new NotImplementedException();
            }
            else
            {
                var sr = new ServiceRequest()
                {
                    Id = Guid.NewGuid(),
                    ServiceId = form.ServiceId.Value,
                    ClaimantName = form.ClaimantName
                };
                db.ServiceRequests.Add(sr);
                await db.SaveChangesAsync();

                return Json(new { Id = sr.Id });
            }
        }



        #endregion
    }
}