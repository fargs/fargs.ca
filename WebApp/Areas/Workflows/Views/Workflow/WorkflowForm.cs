using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Workflows.Views.Workflow
{
    public class WorkflowForm
    {
        public WorkflowForm() { }
        public WorkflowForm(Guid physicianId)
        {
            PhysicianId = physicianId;
        }
        public WorkflowForm(Guid workflowId, Guid physicianId, ImeHubDbContext db) : this(physicianId)
        {
            var workflow = db.Workflows
                .Single(s => s.Id == workflowId);

            WorkflowId = workflowId;
            Name = workflow.Name;
            PhysicianId = physicianId;
        }

        public Guid? WorkflowId { get; set; }
        [Required]
        public Guid PhysicianId { get; set; }
        public string Name { get; set; }
    }
}