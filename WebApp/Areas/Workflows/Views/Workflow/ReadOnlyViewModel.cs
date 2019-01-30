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
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Workflows.Views.Workflow
{
    public class WorkflowViewModel : ViewModelBase
    {
        public WorkflowViewModel() { }
        public WorkflowViewModel(IIdentity identity, DateTime now) : base(identity, now)
        {
            PhysicianId = PhysicianId;
        }
        public WorkflowViewModel(Guid workflowId, ImeHubDbContext db, IIdentity identity, DateTime now) : this(identity, now)
        {
            var workflow = db.Workflows
                .AsNoTracking()
                .AsExpandable()
                .Select(WorkflowModel.FromWorkflow)
                .SingleOrDefault(s => s.Id == workflowId);
            
            WorkflowId = workflowId;
            Name = workflow.Name;
            WorkItems = workflow.WorkItems.Select(wi => new WorkflowViewModel.WorkItemViewModel(wi));
        }

        public Guid? WorkflowId { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<WorkItemViewModel> WorkItems { get; set; }

        public class WorkItemViewModel : LookupViewModel<Guid>
        {
            public WorkItemViewModel()
            {
                Dependencies = new List<WorkItemDependentViewModel>();
            }
            public WorkItemViewModel(WorkflowModel.WorkItemModel workItem) : this()
            {
                Id = workItem.Id;
                WorkflowId = workItem.WorkflowId;
                Sequence = workItem.Sequence;
                Name = workItem.Name;
                ResponsibleRoleId = workItem.ResponsibleRoleId;
                ResponsibleRole = new LookupViewModel<Guid>(workItem.ResponsibleRole);
                Dependencies = workItem.Dependencies.Select(d => new WorkflowViewModel.WorkItemViewModel.WorkItemDependentViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                });
            }
            public Guid WorkflowId { get; set; }
            public short Sequence { get; set; }
            public DateTime? DueDate { get; set; }
            public Guid? ResponsibleRoleId { get; set; }
            public LookupViewModel<Guid> ResponsibleRole { get; set; }
            
            public IEnumerable<WorkItemDependentViewModel> Dependencies { get; set; }

            public class WorkItemDependentViewModel
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}