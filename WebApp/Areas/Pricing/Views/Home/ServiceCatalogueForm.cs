using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Pricing.Views.Home
{
    public class ServiceCatalogueForm : ViewModelBase
    {
        public ServiceCatalogueForm()
        {
        }
        public ServiceCatalogueForm(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(db, PhysicianId.Value);   
        }
        public short ServiceCatalogueId { get; set; }
        public short? CityId { get; set; }
        public short? ServiceId { get; set; }
        public short? CompanyId { get; set; }
        [Required]
        public decimal Price { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private OrvosiDbContext db;
            private Guid physicianId;
            public ViewDataModel(OrvosiDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;
                Cities = GetCitySelectList();
                Services = GetPhysicianServiceSelectList();
                Companies = GetPhysicianCompanySelectList();
            }
            public IEnumerable<SelectListItem> Cities { get; set; }
            public IEnumerable<SelectListItem> Services { get; set; }
            public IEnumerable<SelectListItem> Companies { get; set; }

            private List<SelectListItem> GetCitySelectList()
            {
                var cities = db.Cities
                    .Select(CityDto.FromCityEntity.Expand())
                    .ToList();

                return cities
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString(),
                        Group = new SelectListGroup
                        {
                            Name = d.Province.Name
                        }
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
            private List<SelectListItem> GetPhysicianServiceSelectList()
            {
                var services = db.PhysicianServices
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(c => c.Service)
                    .Select(LookupDto<short>.FromServiceEntity.Expand())
                    .ToList();

                return services
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
            private List<SelectListItem> GetPhysicianCompanySelectList()
            {
                var companies = db.PhysicianCompanies
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(c => c.Company)
                    .Select(LookupDto<short>.FromCompanyEntity.Expand())
                    .ToList();

                return companies
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
        }
    }
}