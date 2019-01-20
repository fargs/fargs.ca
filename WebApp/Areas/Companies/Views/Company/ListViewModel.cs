using System.Collections.Generic;
using WebApp.Views.Shared;
using System.Web.Mvc;
using ImeHub.Data;
using System.Security.Principal;
using System;
using System.Linq;
using ImeHub.Models;
using LinqKit;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ListViewModel : ViewModelBase
    {
        public ListViewModel(Guid? companyId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue)
            {
                throw new ArgumentNullException("PhysicianId is null");
            }
            var companies = db.Companies
                .AsNoTracking()
                .AsExpandable()
                .Where(pc => pc.PhysicianId == PhysicianId)
                .Select(CompanyModel.FromCompany)
                .ToList();

            Companies = companies.Select(s => new CompanyViewModel(s));
            CompanyCount = Companies.Count();
            if (companyId.HasValue)
            {
                SelectedCompanyId = companyId;
                SelectedCompany = Companies.Single(c => c.Id == companyId.Value);
            }
        }
        public IEnumerable<CompanyViewModel> Companies { get; set; }
        public int CompanyCount { get; set; }
        public Guid? SelectedCompanyId { get; private set; }
        public CompanyViewModel SelectedCompany { get; private set; }
    }
}