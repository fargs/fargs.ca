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
    public class UserModel : ContactModel
    {
        public bool EmailConfirmed { get; set; }
        public Guid RoleId { get; set; }
        public bool IsTestRecord { get; set; }
        public string UserName { get; set; }
        public RoleModel Role { get; set; }
        public LookupModel<Guid> AsOwner { get; set; }
        public IEnumerable<LookupModel<Guid>> AsManager { get; set; }
        public IEnumerable<LookupModel<Guid>> AsTeamMember { get; set; }

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
            //Invites = a.PhysicianOwners.Select(pi => new PhysicianInviteModel
            //{
            //    Id = pi.Id,
            //    PhysicianId = pi.PhysicianId,
            //    Physician = LookupModel<Guid>.FromPhysician.Invoke(pi.Physician),
            //    ToName = pi.ToName,
            //    ToEmail = pi.ToEmail,
            //    FromName = pi.FromName,
            //    FromEmail = pi.FromEmail,
            //    LinkClickedDate = pi.LinkClickedDate,
            //    AcceptanceStatus = LookupModel<byte>.FromPhysicianInviteAcceptanceStatus.Invoke(pi.PhysicianInviteAcceptanceStatu),
            //    AcceptanceStatusChangedDate = pi.AcceptanceStatusChangedDate
            //}),
            AsOwner = LookupModel<Guid>.FromPhysician.Invoke(a.Physicians_OwnerId.FirstOrDefault()),
            AsManager = a.Physicians_ManagerId.AsQueryable()
                .Select(LookupModel<Guid>.FromPhysician.Expand()),
            AsTeamMember = a.TeamMembers.AsQueryable()
                .Select(tm => tm.Physician)
                .Select(LookupModel<Guid>.FromPhysician.Expand())
        };
    }
}
