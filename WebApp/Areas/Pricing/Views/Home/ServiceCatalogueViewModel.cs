using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Pricing.Views.Home
{
    public class ServiceCatalogueViewModel : ViewModelBase
    {
        public ServiceCatalogueViewModel(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            var serviceCatalogues = db.ServiceCatalogues
                .Where(sc => sc.PhysicianId == PhysicianId)
                .Select(sc => new ServiceCatalogueDto
                {
                    PhysicianId = sc.PhysicianId.Value,
                    CityId = sc.LocationId,
                    //City = CityDto.FromCityEntity.Invoke(sc.City),
                    City = !sc.LocationId.HasValue ? null : new CityDto
                    {
                        Id = sc.City.Id,
                        Name = sc.City.Name,
                        Code = sc.City.Code
                    },
                    ServiceId = sc.ServiceId,
                    //Service = LookupDto<short>.FromServiceEntity.Invoke(sc.Service),
                    Service = !sc.ServiceId.HasValue ? null : new LookupDto<short>
                    {
                        Id = sc.Service.Id,
                        Name = sc.Service.Name,
                        Code = sc.Service.Code,
                        ColorCode = sc.Service.ColorCode
                    },
                    CompanyId = sc.CompanyId,
                    //Company = LookupDto<short>.FromCompanyEntity.Invoke(sc.Company),
                    Company = !sc.CompanyId.HasValue ? null : new LookupDto<short>
                    {
                        Id = sc.Company.Id,
                        Name = sc.Company.Name,
                        Code = sc.Company.Code,
                        ColorCode = ""
                    },
                    Price = sc.Price ?? 0.0M
                })
                .ToList();

            var viewModel = serviceCatalogues.Select(p => new
            {
                CityId = p.CityId,
                CityLabel = p.CityId.HasValue ? p.City.Name : "(default)",
                ServiceId = p.ServiceId,
                ServiceLabel = p.ServiceId.HasValue ? p.Service.Name : "(default)",
                CompanyId = p.CompanyId,
                CompanyLabel = p.CompanyId.HasValue ? p.Company.Name : "(default)",
                Price = p.Price
            });

            CityPrices = viewModel
                .GroupBy(sc => new
                {
                    sc.CityId,
                    sc.CityLabel
                })
                .Select(c => new CityPriceViewModel
                {
                    CityId = c.Key.CityId,
                    CityLabel = c.Key.CityLabel,
                    Services = c.GroupBy(s => new {
                        s.ServiceId,
                        s.ServiceLabel
                    })
                    .Select(s => new ServicePriceViewModel
                    {
                        ServiceId = s.Key.ServiceId,
                        ServiceLabel = s.Key.ServiceLabel,
                        Companies = s.Select(cp => new CompanyPriceViewModel
                        {
                            CompanyId = cp.CompanyId,
                            CompanyLabel = cp.CompanyLabel,
                            Price = cp.Price
                        })
                        .OrderBy(cp => cp.CompanyLabel)
                    })
                    .OrderBy(cp => cp.ServiceLabel)
                })
                .OrderBy(c => c.CityLabel);

            ServicePrices = viewModel
                .GroupBy(sc => new
                {
                    sc.ServiceId,
                    sc.ServiceLabel
                })
                .Select(c => new ServicePriceViewModel
                {
                    ServiceId = c.Key.ServiceId,
                    ServiceLabel = c.Key.ServiceLabel,
                    Cities = c.GroupBy(s => new {
                        s.CityId,
                        s.CityLabel
                    })
                    .Select(s => new CityPriceViewModel
                    {
                        CityId = s.Key.CityId,
                        CityLabel = s.Key.CityLabel,
                        Companies = s.Select(cp => new CompanyPriceViewModel
                        {
                            CompanyId = cp.CompanyId,
                            CompanyLabel = cp.CompanyLabel,
                            Price = cp.Price
                        })
                        .OrderBy(cp => cp.CompanyLabel)
                    })
                    .OrderBy(cp => cp.CityLabel)
                })
                .OrderBy(c => c.ServiceLabel);

            ViewData = new ViewDataModel(db);
        }


        public IEnumerable<CityPriceViewModel> CityPrices { get; private set; }
        public IEnumerable<ServicePriceViewModel> ServicePrices { get; private set; }
        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private OrvosiDbContext db;
            public ViewDataModel(OrvosiDbContext db)
            {
                this.db = db;
            }
            public IEnumerable<LookupViewModel<short>> Locations { get; set; }
            public IEnumerable<LookupViewModel<short>> Services { get; set; }

        }
        
        public class CityPriceViewModel
        {
            public short? CityId { get; set; }
            public string CityLabel { get; set; }
            public IEnumerable<ServicePriceViewModel> Services { get; set; }
            public IEnumerable<CompanyPriceViewModel> Companies { get; set; }
        }
        public class ServicePriceViewModel
        {
            public short? ServiceId { get; set; }
            public string ServiceLabel { get; set; }
            public IEnumerable<CityPriceViewModel> Cities { get; set; }
            public IEnumerable<CompanyPriceViewModel> Companies { get; set; }
        }
        public class CompanyPriceViewModel
        {
            public short? CompanyId { get; set; }
            public string CompanyLabel { get; set; }
            public decimal? Price { get; set; }
        }
    }
}