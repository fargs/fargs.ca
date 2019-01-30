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
    public class WorkflowController : BaseController
    {
        private ImeHubDbContext db;

        public WorkflowController(ImeHubDbContext db, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
        }
        [AuthorizeRole(Feature = Features.Workflows.Section)]
        public ViewResult Index(Guid? workflowId)
        {
            var list = new ListViewModel(workflowId, db, identity, now);

            WorkflowViewModel readOnly = null;
            if (workflowId.HasValue)
            {
                readOnly = new WorkflowViewModel(workflowId.Value, db, identity, now);
            }

            var viewModel = new IndexViewModel(list, readOnly, identity, now);

            return View(viewModel);
        }

        
        [AuthorizeRole(Feature = Features.Workflows.Manage)]
        public PartialViewResult List(Guid? workflowId)
        {
            var viewModel = new ListViewModel(workflowId, db, identity, now);

            return PartialView(viewModel);
        }

        [AuthorizeRole(Feature = Features.Workflows.Manage)]
        public PartialViewResult ShowNewWorkflowForm()
        {
            if (!physicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var formModel = new WorkflowForm(physicianId.Value);

            return PartialView("WorkflowForm", formModel);
        }

        [AuthorizeRole(Feature = Features.Workflows.Section)]
        public PartialViewResult ReadOnly(Guid workflowId)
        {
            var readOnly = new WorkflowViewModel(workflowId, db, identity, now);

            return PartialView(readOnly);
        }
        [HttpPost]
        [AuthorizeRole(Feature = Features.Workflows.Manage)]
        public async Task<ActionResult> SaveNewWorkflowForm(WorkflowForm form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("WorkflowForm", form);
            }

            var workflow = new Workflow
            {
                Id = Guid.NewGuid(),
                PhysicianId = form.PhysicianId,
                Name = form.Name
            };
            db.Workflows.Add(workflow);
            await db.SaveChangesAsync();

            return Json(new
            {
                id = workflow.Id
            });
        }

        [HttpPost]
        [AuthorizeRole(Feature = Features.Workflows.Manage)]
        public async Task<ActionResult> SaveEditWorkflowForm(WorkflowForm form)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("WorkflowForm", form);
            }

            var workflow = db.Workflows.Single(s => s.Id == form.WorkflowId);
            workflow.Name = form.Name;

            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}