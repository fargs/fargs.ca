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
using WebApp.Models;
using WebApp.Views.Shared;
using ImeHub.Models;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class ReadOnlyViewModel : ViewModelBase
    {
        public ReadOnlyViewModel() { }
        public ReadOnlyViewModel(Guid physicianId, ImeHubDbContext db, IIdentity identity, DateTime now)
        {
            var physician = db.Physicians
                .AsNoTracking()
                .AsExpandable()
                .Select(PhysicianModel.FromPhysician)
                .SingleOrDefault(s => s.Id == physicianId);
            
            Id = physician.Id.ToString();
            CompanyName = physician.CompanyName;
            OwnerId = physician.OwnerId;
            Code = physician.Code;
            ColorCode = physician.ColorCode;

            OwnerInvites = physician.Invites.Select(i => new PhysicianInviteViewModel(i));
            Owner = physician.Owner == null ? null : new ContactViewModel(physician.Owner);
            Manager = physician.Manager == null ? null : new ContactViewModel(physician.Manager);
        }

        public string Id { get; set; }
        public string CompanyName { get; set; }
        public Guid? OwnerId { get; set; }
        public string Code { get; set; }
        public string ColorCode { get; set; }

        public ContactViewModel Owner { get; set; }
        public IEnumerable<PhysicianInviteViewModel> OwnerInvites { get; set; }
        public ContactViewModel Manager { get; set; }
    }
}