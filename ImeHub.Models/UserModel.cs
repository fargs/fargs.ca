using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data = ImeHub.Data;

namespace ImeHub.Models
{
    public class UserModel : ContactModel
    {
        public bool EmailConfirmed { get; set; }
        public Guid RoleId { get; set; }
        public bool IsTestRecord { get; set; }
        public string UserName { get; set; }
        public string EmailProvider { get; set; }
        public string EmailProviderCredential { get; set; }
        public RoleModel Role { get; set; }
        public IEnumerable<LookupModel<Guid>> AsOwner { get; set; }
        public IEnumerable<LookupModel<Guid>> AsManager { get; set; }
        public IEnumerable<LookupModel<Guid>> AsTeamMember { get; set; }
        public WorkflowModel Setup { get; set; }

        #region Computed Properties
        public bool IsEmailProviderSet => !string.IsNullOrEmpty(EmailProvider);
        #endregion

        public partial class RoleModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string ColorCode { get; set; }

            public virtual IEnumerable<FeatureModel> Features { get; set; }

            public RoleModel()
            {
                Features = new List<FeatureModel>();
            }

            public class FeatureModel
            {
                public Guid RoleFeatureId { get; set; }
                public bool IsActive { get; set; }
                public Guid FeatureId { get; set; }
                public string Name { get; set; }
            }
        }

        public static new Expression<Func<Data.User, UserModel>> FromUser = a => new UserModel
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Title = a.Title,
            ColorCode = a.ColorCode,
            Email = a.Email,
            EmailProvider = a.EmailProvider,
            EmailProviderCredential = a.EmailProviderCredential,
            Role = new RoleModel
            {
                Id = a.Role.Id,
                Name = a.Role.Name,
                Code = a.Role.Code,
                ColorCode = a.Role.ColorCode,
                Features = a.Role.RoleFeatures.Select(rf => new RoleModel.FeatureModel
                {
                    FeatureId = rf.Feature.Id,
                    Name = rf.Feature.Name
                })
            },
            //Setup = a.UserSetupWorkflow == null ? null : new UserSetupWorkflowModel
            //{
            //    Id = a.UserSetupWorkflow.Id,
            //    Name = a.UserSetupWorkflow.Name,

            //    StatusId = (Enums.WorkflowStatus)a.UserSetupWorkflow.StatusId,
            //    Status = StatusModel<Enums.WorkflowStatus>.FromUserSetupWorkflow.Invoke(a.UserSetupWorkflow),
            //    WorkItems = a.UserSetupWorkflow.UserSetupWorkItems.Select(wi => new UserSetupWorkflowModel.UserSetupWorkItemModel
            //    {
            //        Id = wi.Id,
            //        Name = wi.Name,
            //        Sequence = wi.Sequence,
            //        StatusId = (Enums.WorkItemStatus)wi.StatusId,
            //        Status = StatusModel<Enums.WorkItemStatus>.FromUserSetupWorkItem.Invoke(wi),
            //        AssignedToId = wi.AssignedToId,
            //        AssignedToChangedById = wi.AssignedToChangedById,
            //        AssignedToChangedDate = wi.AssignedToChangedDate,
            //        ResponsibleRoleId = wi.ResponsibleRoleId,
            //        DueDate = wi.DueDate
            //    })
            //},
            //AsOwner = a.PhysicianOwners.AsQueryable()
            //    .Select(po => po.Physician)
            //    .Select(LookupModel<Guid>.FromPhysician.Expand()),
            //AsManager = a.Physicians_ManagerId.AsQueryable()
            //    .Select(LookupModel<Guid>.FromPhysician.Expand()),
            AsTeamMember = a.TeamMembers.AsQueryable()
                .Select(tm => tm.Physician)
                .Select(LookupModel<Guid>.FromPhysician.Expand())
        };
    }
}
