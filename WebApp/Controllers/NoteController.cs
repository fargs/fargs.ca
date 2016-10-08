using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class NoteController : Controller
    {
        [HttpGet]
        // GET: Note
        public async Task<ActionResult> EditNote(int serviceRequestId)
        {
            var context = new Orvosi.Data.OrvosiDbContext();
            var editForm = await context.ServiceRequests
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
        public async Task<ActionResult> UpdateNote(NoteEditForm form)
        {
            var context = new Orvosi.Data.OrvosiDbContext();

            var target = context.ServiceRequests.Find(form.ServiceRequestId);
            target.Notes = form.Notes;
            target.ModifiedUser = User.Identity.Name;
            target.ModifiedDate = SystemTime.Now();

            await context.SaveChangesAsync();
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