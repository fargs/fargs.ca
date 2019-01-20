using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImeHub.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class PricingMatrixViewModel
    {
        public PricingMatrixViewModel(ImeHubDbContext db, CompanyModel company, Guid physicianId)
        {
            CompanyId = company.Id.ToString();
            CancellationPolicy = new CancellationPolicyViewModel(company);
            Services = company.Services
                .Where(s => s.IsTravelRequired)
                .Select(cs => new ServiceViewModel(cs))
                .OrderBy(s => s.Name)
                .ToArray();
            ViewData = new ViewDataModel(db, company, physicianId);
        }
        public string CompanyId { get; set; }
        public CancellationPolicyViewModel CancellationPolicy { get; set; }
        public IEnumerable<ServiceViewModel> Services { get; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private CompanyModel company;
            private Guid physicianId;

            public IEnumerable<CityModel> Cities { get; }

            public ViewDataModel(ImeHubDbContext db, CompanyModel company, Guid physicianId)
            {
                this.db = db;
                this.company = company;
                this.physicianId = physicianId;
                Cities = GetCities();
            }

            private IEnumerable<CityModel> GetCities()
            {
                var cities = company.Addresses.Select(c => c.CityId).Distinct().ToArray();
                return db.Cities
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(c => cities.Contains(c.Id))
                    .Select(CityModel.FromCity)
                    .OrderBy(c => c.Name)
                    .ToList();
            }
        }

    }
}