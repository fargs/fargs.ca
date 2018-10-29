using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using Orvosi.Data;
using System.Security.Principal;
using System;
using System.Linq;
using WebApp.Models;
using LinqKit;

namespace WebApp.Areas.Services.Views.Service
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var servicesDto = db.ServiceV2
                .Where(pc => pc.PhysicianId == PhysicianId)
                .Select(ServiceV2Dto.FromServiceV2Entity.Expand())
                .ToList();

            Services = servicesDto.Select(s => new ServiceV2ViewModel(s));
            ServiceCount = Services.Count();
        }
        public IEnumerable<ServiceV2ViewModel> Services { get; set; }
        public int ServiceCount { get; set; }
    }
}