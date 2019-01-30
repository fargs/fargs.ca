using LinqKit;
using ImeHub.Data;
using Enums = ImeHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Workflows.Views.Workflow
{
    public class WorkItemFormModel
    {
        public WorkItemFormModel()
        {
        }
        public WorkItemFormModel(WorkflowModel.WorkItemModel workItem, Guid physicianId, ImeHubDbContext db)
        {
            Id = workItem.Id;
            WorkflowId = workItem.WorkflowId;
            Name = workItem.Name;
            Dependencies = new string[] { };
            ViewData = new ViewDataModel(db, physicianId, workItem.WorkflowId, workItem.Id);
        }
        public WorkItemFormModel(Guid workflowId, Guid physicianId, ImeHubDbContext db)
        {
            WorkflowId = workflowId;
            Dependencies = new string[] { };
            ViewData = new ViewDataModel(db, physicianId, workflowId, null);
        }
        public Guid? Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public Guid ResponsibleRoleId { get; set; }
        public IEnumerable<string> Dependencies { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;
            private Guid workflowId;
            private Guid? workItemId;

            public ViewDataModel(ImeHubDbContext db, Guid physicianId, Guid workflowId, Guid? workItemId)
            {
                this.db = db;
                this.physicianId = physicianId;
                this.workflowId = workflowId;

                Roles = GetRoleSelectList();
                WorkItems = GetWorkItemsSelectList();
            }

            public IEnumerable<SelectListItem> Roles { get; set; }
            private IEnumerable<SelectListItem> GetRoleSelectList()
            {
                return db.TeamRoles
                    .Where(tr => tr.PhysicianId == physicianId)
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }
            public IEnumerable<SelectListItem> WorkItems { get; set; }
            private IEnumerable<SelectListItem> GetWorkItemsSelectList()
            {
                var workItems = db.WorkItems
                    .Where(wf => wf.WorkflowId == workflowId);

                if (workItemId.HasValue)
                {
                    workItems = workItems.Where(wi => wi.Id != workItemId);
                }
                
                return workItems
                    .OrderBy(wi => wi.Sequence)
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToList();
            }
        }
    }
}