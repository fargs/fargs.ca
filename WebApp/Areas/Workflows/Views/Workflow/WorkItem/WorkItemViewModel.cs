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
            
        }
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public Guid ResponsibleRoleId { get; set; }
        public LookupViewModel<Guid> ResponsibleRole { get; set; }
    }
}