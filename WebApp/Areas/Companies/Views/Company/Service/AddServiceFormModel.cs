using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using WebApp.Library;
using ImeHub.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class AddServiceFormModel
    {
        public AddServiceFormModel()
        {
        }
        public AddServiceFormModel(Guid companyId, Guid? serviceId, ImeHubDbContext db, Guid physicianId)
        {

            CompanyId = companyId;
            if (serviceId.HasValue)
            {
                ServiceId = serviceId;
                var selectedServiceDto = ServiceModel.FromServiceEntity.Invoke(db.Services.SingleOrDefault(s => s.Id == serviceId.Value));
                SelectedServiceName = selectedServiceDto.Name;
                SelectedServicePrice = selectedServiceDto.Price;
                SelectedServiceIsTravelRequired = selectedServiceDto.IsTravelRequired;
            }
            ViewData = new ViewDataModel(db, physicianId);
        }
        public string SelectedServiceName { get; set; }
        public decimal? SelectedServicePrice { get; set; }
        public bool? SelectedServiceIsTravelRequired { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public bool? IsTravelRequired { get; set; }

        public ViewDataModel ViewData { get; private set; }

        public void LoadViewData(ImeHubDbContext db, Guid physicianId)
        {
            ViewData = new ViewDataModel(db, physicianId);
        }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;

            public IEnumerable<SelectListItem> Services { get; }

            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;
                Services = GetServiceSelectList();
            }

            private IEnumerable<SelectListItem> GetServiceSelectList()
            {
                return db.Companies
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(s => s.PhysicianId == physicianId)
                    .SelectMany(c => c.Services, (c, s) => new
                    {
                        s.Name,
                        s.IsTravelRequired,
                        s.Price
                    })
                    .Distinct()
                    .AsEnumerable()
                    .Select(c => new SelectListItem()
                    {
                        Text = $"{c.Name} ({String.Format("{0:C2}", c.Price.ToString())})",
                        Value = c.Name
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }
        }
    }
}