using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Library.Filters;
using Orvosi.Data;
using System.Security.Principal;

namespace WebApp.Controllers
{
    public class NoteController : BaseController
    {
        private OrvosiDbContext db;

        public NoteController(OrvosiDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }

        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageInvoiceNote)]
        public async Task<ActionResult> EditNote(int serviceRequestId)
        {
            var editForm = await db.ServiceRequests
                .Where(sr => sr.Id == serviceRequestId)
                .Select(sr => new NoteEditForm
                {
                    ServiceRequestId = sr.Id,
                    ClaimantName = sr.ClaimantName,
                    Notes = sr.Notes
                }).FirstAsync();

            return Json(editForm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.ServiceRequest.ManageInvoiceNote)]
        public async Task<ActionResult> UpdateNote(NoteEditForm form)
        {
            var target = db.ServiceRequests.Find(form.ServiceRequestId);
            target.Notes = form.Notes;
            target.ModifiedUser = loggedInUserId.ToString();
            target.ModifiedDate = now;

            await db.SaveChangesAsync();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }

    public class NoteEditForm
    {
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public string Notes { get; set; }
    }
}