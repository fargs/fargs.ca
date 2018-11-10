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
        public virtual ContactModel Owner { get; set; } // FK_Physician_Owner

        public virtual IEnumerable<PhysicianInviteModel> Invites { get; set; }

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

        public static new Expression<Func<Data.Physician, PhysicianModel>> FromPhysician = a => a == null ? null : new PhysicianModel
        {
            Id = a.Id,
            CompanyName = a.CompanyName,
            Code = a.Code,
            ColorCode = a.ColorCode,
            ManagerId = a.ManagerId,
            Manager = ContactModel.FromUser.Invoke(a.Manager),
            OwnerId = a.OwnerId,
            Owner = ContactModel.FromUser.Invoke(a.Owner),
            Invites = a.PhysicianInvites.Select(pi => new PhysicianInviteModel
            {
                Id = pi.Id,
                PhysicianId = pi.PhysicianId,
                Physician = LookupModel<Guid>.FromPhysician.Invoke(pi.Physician),
                ToEmail = pi.ToEmail,
                ToName = pi.ToName,
                FromEmail = pi.FromEmail,
                FromName = pi.FromName,
                SentDate = pi.SentDate,
                LinkClickedDate = pi.LinkClickedDate,
                AcceptanceStatusId = pi.AcceptanceStatusId,
                AcceptanceStatus = LookupModel<byte>.FromPhysicianInviteAcceptanceStatus.Invoke(pi.PhysicianInviteAcceptanceStatu)
            })
        };
    }
}
