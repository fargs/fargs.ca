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
    public class UserSetupWorkflowModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Enums.WorkflowStatus StatusId { get; set; }
        public StatusModel<Enums.WorkflowStatus> Status { get; set; }
        public IEnumerable<UserSetupWorkItemModel> WorkItems { get; set; }

        public class UserSetupWorkItemModel : LookupModel<Guid>
        {
            public UserSetupWorkItemModel()
            {
                Dependencies = new List<WorkItemDependentModel>();
            }
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
    }
}
