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
    public class PhysicianModel
    {
        public Guid Id { get; set; } // Id (Primary key)
        public string CompanyName { get; set; } // CompanyName (length: 250)
        public string Code { get; set; } // Code (length: 10)
        public string ColorCode { get; set; } // ColorCode (length: 10)
        public Guid? OwnerId { get; set; } // OwnerId
        public Guid ManagerId { get; set; } // ManagerId

        /// <summary>
        /// Parent User pointed by [Physician].([ManagerId]) (FK_Physician_Manager)
        /// </summary>
        public virtual ContactModel Manager { get; set; } // FK_Physician_Manager

        /// <summary>
        /// Parent User pointed by [Physician].([OwnerId]) (FK_Physician_Owner)
        /// </summary>
        public virtual PhysicianOwnerModel Owner { get; set; } // FK_Physician_Owner

        public virtual IEnumerable<PhysicianInviteLogModel> Invites { get; set; }

        public class PhysicianOwnerModel
        {
            public string Email { get; set; } // Email (length: 128)
            public string Name { get; set; } // Name (length: 128)

            public byte AcceptanceStatusId { get; set; } // AcceptanceStatusId
            public DateTime AcceptanceStatusChangedDate { get; set; }
            public virtual LookupModel<byte> AcceptanceStatus { get; set; }

            public System.Guid? UserId { get; set; } // UserId
            public virtual ContactModel User { get; set; } 
        }
        public class PhysicianInviteLogModel
        {
            public Guid Id { get; set; }
            public string To { get; set; }
            public string Cc { get; set; }
            public string Bcc { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public DateTime SentDate { get; set; }
        }

        public static Expression<Func<Data.Physician, PhysicianModel>> FromPhysician = a => a == null ? null : new PhysicianModel
        {
            Id = a.Id,
            CompanyName = a.CompanyName,
            Code = a.Code,
            ColorCode = a.ColorCode,
            ManagerId = a.ManagerId,
            Manager = ContactModel.FromUser.Invoke(a.Manager),
            Owner = a.PhysicianOwner == null ? null : new PhysicianModel.PhysicianOwnerModel()
            {
                Email = a.PhysicianOwner.Email,
                Name = a.PhysicianOwner.Name,
                AcceptanceStatusId = a.PhysicianOwner.AcceptanceStatusId,
                AcceptanceStatusChangedDate = a.PhysicianOwner.AcceptanceStatusChangedDate,
                AcceptanceStatus = new LookupModel<byte>
                {
                    Id = a.PhysicianOwner.PhysicianOwnerAcceptanceStatu.Id,
                    Name = a.PhysicianOwner.PhysicianOwnerAcceptanceStatu.Name,
                    Code = a.PhysicianOwner.PhysicianOwnerAcceptanceStatu.Code,
                    ColorCode = a.PhysicianOwner.PhysicianOwnerAcceptanceStatu.ColorCode,
                },
                UserId = a.PhysicianOwner.UserId,
                User = ContactModel.FromUser.Invoke(a.PhysicianOwner.User),
            },
            Invites = a.PhysicianInviteLogs.Select(pi => new PhysicianInviteLogModel
            {
                Id = pi.Id,
                To = pi.To,
                Cc = pi.Cc,
                Bcc = pi.Bcc,
                Subject = pi.Subject,
                Body = pi.Body
            })
        };
    }
}
