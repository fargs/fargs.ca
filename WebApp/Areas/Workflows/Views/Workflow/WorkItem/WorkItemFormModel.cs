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
            ViewData = new ViewDataModel(db, physicianId);
        }
        public WorkItemFormModel(Guid workflowId, Guid physicianId, ImeHubDbContext db)
        {
            WorkflowId = workflowId;
            ViewData = new ViewDataModel(db, physicianId);
        }
        public Guid? Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public Guid ResponsibleRoleId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;

            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;

                Roles = GetRoleSelectList();
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
        }
    }
}