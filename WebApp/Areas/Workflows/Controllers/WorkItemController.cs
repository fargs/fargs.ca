using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Shared;
using WebApp.Areas.Workflows.Views.Workflow;
using WebApp.Library.Filters;
using Features = ImeHub.Models.Enums.Features.PhysicianPortal;

namespace WebApp.Areas.Workflows.Controllers
{
    public class WorkItemController : BaseController
    {
        private ImeHubDbContext db;
        public WorkItemController(ImeHubDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }

        #region Get

        [AuthorizeRole(Feature = Features.Workflows.Manage)]
        public PartialViewResult Create(Guid workflowId)
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new WorkItemFormModel(workflowId, physicianId.Value, db);

            return PartialView("WorkItemForm", formModel);
        }

        #endregion

        #region Posts

        [HttpPost]
        [AuthorizeRole(Feature = Features.Workflows.Manage)]
        public async Task<ActionResult> Create(WorkItemFormModel form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("WorkItemForm", form);
            }

            var workItem = new WorkItem
            {
                Id = Guid.NewGuid(),
                WorkflowId = form.WorkflowId,
                Name = form.Name,
                TeamRoleId = form.ResponsibleRoleId
            };

            var dependencies = Request.Form.GetValues("Dependencies") == null ? new string[0] : Request.Form.GetValues("Dependencies");
            foreach (var id in dependencies)
            {
                var dependent = new WorkItemRelated
                {
                    ParentId = workItem.Id,
                    ChildId = new Guid(id)
                };
                workItem.WorkItemRelateds_ParentId.Add(dependent);
            }
            db.WorkItems.Add(workItem);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = workItem.Id,
                workflowId = workItem.WorkflowId
            });
        }

        #endregion

    }
}