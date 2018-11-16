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
        public IEnumerable<PhysicianInviteModel> Invites { get; set; }
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

        public class PhysicianInviteModel
        {
            public Guid Id { get; set; }
            public Guid PhysicianId { get; set; } // PhysicianId
            public LookupModel<Guid> Physician { get; set; }
            public string ToName { get; set; }
            public string ToEmail { get; set; }
            public string FromName { get; set; }
            public string FromEmail { get; set; }
            public DateTime? SentDate { get; set; } // SentDate
            public DateTime? LinkClickedDate { get; set; } // LinkClickedDate
            public byte AcceptanceStatusId { get; set; }
            public LookupModel<byte> AcceptanceStatus { get; set; }
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
            Invites = a.PhysicianInvites.Select(pi => new PhysicianInviteModel
            {
                Id = pi.Id,
                PhysicianId = pi.PhysicianId,
                Physician = LookupModel<Guid>.FromPhysician.Invoke(pi.Physician),
                ToName = pi.ToName,
                ToEmail = pi.ToEmail,
                FromName = pi.FromName,
                FromEmail = pi.FromEmail,
                SentDate = pi.SentDate,
                LinkClickedDate = pi.LinkClickedDate,
                AcceptanceStatusId = pi.AcceptanceStatusId,
                AcceptanceStatus = LookupModel<byte>.FromPhysicianInviteAcceptanceStatus.Invoke(pi.PhysicianInviteAcceptanceStatu)
            }),
            AsOwner = LookupModel<Guid>.FromPhysician.Invoke(a.Physicians_OwnerId.FirstOrDefault()),
            AsManager = a.Physicians_ManagerId.AsQueryable()
                .Select(LookupModel<Guid>.FromPhysician.Expand()),
            AsTeamMember = a.TeamMembers.AsQueryable()
                .Select(tm => tm.Physician)
                .Select(LookupModel<Guid>.FromPhysician.Expand())
        };
    }
}
