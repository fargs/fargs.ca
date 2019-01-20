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
using WebApp.Areas.Dashboard.Views.Home;
using WebApp.Views.Shared;
using ImeHub.Models;
using Enums = ImeHub.Models.Enums;
using WebApp.Library.Extensions;
using WebApp.Areas.Physicians.Views.Physician.Address;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class ReadOnlyViewModel : ViewModelBase
    {
        public ReadOnlyViewModel() { }
        public ReadOnlyViewModel(PhysicianModel physician)
        {
            SetProperties(physician);
        }
        public ReadOnlyViewModel(Guid physicianId, ImeHubDbContext db, IIdentity identity, DateTime now)
        {
            var physician = db.Physicians
                .AsNoTracking()
                .AsExpandable()
                .Select(PhysicianModel.FromPhysician)
                .SingleOrDefault(s => s.Id == physicianId);

            SetProperties(physician);
        }

        public string Id { get; set; }
        public string CompanyName { get; set; }
        public Guid? OwnerId { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public OwnerViewModel Owner { get; set; }
        public IEnumerable<PhysicianInviteViewModel> OwnerInvites { get; set; }
        public ContactViewModel Manager { get; set; }
        public AddressListViewModel AddressList { get; set; }

        private void SetProperties(PhysicianModel physician)
        {
            Id = physician.Id.ToString();
            CompanyName = physician.CompanyName;
            OwnerId = physician.OwnerId;
            Code = physician.Code;
            ColorCode = physician.ColorCode;

            OwnerInvites = physician.Invites.Select(i => new PhysicianInviteViewModel(physician));
            Owner = physician.Owner == null ? null : new OwnerViewModel
            {
                Email = physician.Owner.Email,
                Name = physician.Owner.DisplayName,
                AcceptanceStatusId = (Enums.AcceptanceStatus)physician.Owner.AcceptanceStatusId,
                AcceptanceStatusChangedDate = physician.Owner.AcceptanceStatusChangedDate.ToOrvosiDateTimeFormat(),
                AcceptanceStatus = new LookupViewModel<byte>
                {
                    Id = physician.Owner.AcceptanceStatus.Id,
                    Name = physician.Owner.AcceptanceStatus.Name,
                    Code = physician.Owner.AcceptanceStatus.Code,
                    ColorCode = physician.Owner.AcceptanceStatus.ColorCode
                },
                UserId = physician.Owner.UserId,
                User = physician.Owner.UserId.HasValue ? new ContactViewModel(physician.Owner.User) : null
            };
            Manager = physician.Manager == null ? null : new ContactViewModel(physician.Manager);
            AddressList = new AddressListViewModel(physician);
        }

        public class OwnerViewModel
        {
            public string Email { get; set; } // Email (length: 128)
            public string Name { get; set; } // Name (length: 128)

            public Enums.AcceptanceStatus AcceptanceStatusId { get; set; } // AcceptanceStatusId
            public virtual LookupViewModel<byte> AcceptanceStatus { get; set; }
            public string AcceptanceStatusChangedDate { get; set; }

            public Guid? UserId { get; set; } // UserId
            public virtual ContactViewModel User { get; set; }
        }
    }
}