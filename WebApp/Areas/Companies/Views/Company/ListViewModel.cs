using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using Orvosi.Data;
using System.Security.Principal;
using System;
using System.Linq;
using WebApp.Models;
using LinqKit;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(Guid? companyId, OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var companies = db.CompanyV2
                .Where(pc => pc.PhysicianId == PhysicianId)
                .Select(CompanyV2Dto.FromCompanyV2Entity.Expand())
                .ToList();

            Companies = companies.Select(s => new CompanyV2ViewModel(s));
            CompanyCount = Companies.Count();
            if (companyId.HasValue)
            {
                SelectedCompanyId = companyId;
                SelectedCompany = Companies.Single(c => c.Id == companyId.Value);
            }
        }
        public IEnumerable<CompanyV2ViewModel> Companies { get; set; }
        public int CompanyCount { get; set; }
        public Guid? SelectedCompanyId { get; private set; }
        public CompanyV2ViewModel SelectedCompany { get; private set; }
    }
}