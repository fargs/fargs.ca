using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class AddServiceFormModel
    {
        public AddServiceFormModel()
        {
        }
        public AddServiceFormModel(Guid companyId, Guid? serviceId, OrvosiDbContext db, Guid physicianId)
        {

            CompanyId = companyId;
            if (serviceId.HasValue)
            {
                ServiceId = serviceId;
                var selectedServiceDto = ServiceV2Dto.FromServiceV2Entity.Invoke(db.ServiceV2.SingleOrDefault(s => s.Id == serviceId.Value));
                SelectedService = new ServiceV2ViewModel(selectedServiceDto);
            }
            ViewData = new ViewDataModel(db, physicianId);
        }
        public ServiceV2ViewModel SelectedService { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool? IsTravelRequired { get; set; }

        public ViewDataModel ViewData { get; private set; }
        
        public void LoadViewData(OrvosiDbContext db, Guid physicianId)
        {
            ViewData = new ViewDataModel(db, physicianId);
        }

        public class ViewDataModel
        {
            private OrvosiDbContext db;
            private Guid physicianId;

            public IEnumerable<SelectListItem> Services { get; }

            public ViewDataModel(OrvosiDbContext db, Guid physicianId)
            {
                this.db = db;
                this.physicianId = physicianId;
                Services = GetServiceSelectList();
            }

            private IEnumerable<SelectListItem> GetServiceSelectList()
            {
                return db.ServiceV2
                    .Where(s => s.PhysicianId == physicianId)
                    .Select(ServiceV2Dto.FromServiceV2Entity.Expand())
                    .AsEnumerable()
                    .Select(c => new SelectListItem()
                    {
                        Text = $"{c.Name} ({String.Format("{0:C2}", c.Price.ToString())})",
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }
        }
    }
}