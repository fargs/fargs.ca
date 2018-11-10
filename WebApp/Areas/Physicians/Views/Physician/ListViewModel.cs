using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using System.Security.Principal;
using System;
using System.Linq;
using ImeHub.Data;
using LinqKit;
using ImeHub.Models;

namespace WebApp.Areas.Physicians.Views.Physician
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(Guid? physicianId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var physicians = db.Physicians
                .AsNoTracking()
                .AsExpandable()
                .Where(p => p.ManagerId == LoggedInUserId || p.OwnerId == LoggedInUserId)
                .Select(PhysicianModel.FromPhysician)
                .ToList();

            Physicians = physicians.Select(s => new PhysicianViewModel(s));
            PhysicianCount = Physicians.Count();
            if (physicianId.HasValue)
            {
                SelectedPhysicianId = physicianId;
                SelectedPhysician = Physicians.Single(c => c.Id == physicianId.Value);
            }
        }
        public IEnumerable<PhysicianViewModel> Physicians { get; set; }
        public int PhysicianCount { get; set; }
        public Guid? SelectedPhysicianId { get; private set; }
        public PhysicianViewModel SelectedPhysician { get; private set; }
    }
}