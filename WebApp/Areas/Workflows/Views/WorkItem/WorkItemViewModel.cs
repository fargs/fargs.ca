using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Workflows.Views.Workflow
{
    public class WorkItemViewModel
    {
        public WorkItemViewModel(WorkflowModel.WorkItemModel workItem)
        {
            Id = workItem.Id;
            WorkflowId = workItem.WorkflowId;
            Name = workItem.Name;
            Sequence = workItem.Sequence;
            ResponsibleRoleId = workItem.ResponsibleRoleId;
            ResponsibleRole = new LookupViewModel<Guid>(workItem.ResponsibleRole);
            Dependencies = workItem.Dependencies.Select(d => new WorkItemDependentViewModel
            {
                Id = d.Id,
                ParentId = d.ParentId,
                Name = d.Name,
                Sequence = d.Sequence,
                ResponsibleRoleId = d.ResponsibleRoleId,
                ResponsibleRole = new LookupViewModel<Guid>(workItem.ResponsibleRole)
            });
        }
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public Guid? ResponsibleRoleId { get; set; }
        public LookupViewModel<Guid> ResponsibleRole { get; set; }

        public IEnumerable<WorkItemDependentViewModel> Dependencies { get; set; }

        public class WorkItemDependentViewModel
        {
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
            public string Name { get; set; }
            public short Sequence { get; set; }
            public Guid ResponsibleRoleId { get; set; }
            public LookupViewModel<Guid> ResponsibleRole { get; set; }
        }
    }
}