using ImeHub.Data;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data = ImeHub.Data;

namespace ImeHub.Models
{
    public class WorkflowModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PhysicianId { get; set; }
        public Enums.WorkflowStatus StatusId { get; set; }
        public StatusModel<Enums.WorkflowStatus> Status { get; set; }
        public IEnumerable<WorkItemModel> WorkItems { get; set; }

        public class WorkItemModel : LookupModel<Guid>
        {
            public WorkItemModel()
            {
                Dependencies = new List<WorkItemDependentModel>();
            }
            public Guid WorkflowId { get; set; }
            public short Sequence { get; set; }
            public Enums.WorkItemStatus StatusId { get; set; }
            public StatusModel<Enums.WorkItemStatus> Status { get; set; }
            public DateTime? DueDate { get; set; }
            public Guid? AssignedToId { get; set; }
            public PersonModel AssignedTo { get; set; }
            public Guid? AssignedToChangedById { get; set; }
            public DateTime? AssignedToChangedDate { get; set; }
            public Guid? ResponsibleRoleId { get; set; }
            public RoleModel ResponsibleRole { get; set; }

            public IEnumerable<WorkItemDependentModel> Dependencies { get; set; }

            public class WorkItemDependentModel
            {
                public Guid Id { get; set; }
                public DateTime? CompletedDate { get; set; }
                public bool IsObsolete { get; set; }
            }
        }


        public static Expression<Func<Data.Workflow, WorkflowModel>> FromWorkflow = a => a == null ? null : new WorkflowModel
        {
            Id = a.Id,
            Name = a.Name,
            PhysicianId = a.PhysicianId,
            WorkItems = a.WorkItems.Select(wi => new WorkflowModel.WorkItemModel
            {
                Id = wi.Id,
                Name = wi.Name,
                Sequence = wi.Sequence,
                ResponsibleRoleId = wi.RoleId
            })
        };
    }
}
