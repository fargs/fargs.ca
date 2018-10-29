using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class PricingMatrixViewModel
    {
        public PricingMatrixViewModel(OrvosiDbContext db, CompanyV2Dto company, Guid physicianId)
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
            private OrvosiDbContext db;
            private CompanyV2Dto company;
            private Guid physicianId;

            public IEnumerable<CityDto> Cities { get; }

            public ViewDataModel(OrvosiDbContext db, CompanyV2Dto company, Guid physicianId)
            {
                this.db = db;
                this.company = company;
                this.physicianId = physicianId;
                Cities = GetCities();
            }

            private IEnumerable<CityDto> GetCities()
            {
                var cities = company.Addresses.Select(c => c.CityId).Distinct().ToArray();
                return db.Cities
                    .Where(c => cities.Contains(c.Id))
                    .Select(CityDto.FromCityEntity.Expand())
                    .OrderBy(c => c.Name)
                    .ToList();
            }
        }

    }
}