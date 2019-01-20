using FluentDateTime;
using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using ImeHub.Models;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;
using WebApp.Areas.Availability.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AddDayFormModel : ViewModelBase
    {
        public AddDayFormModel()
        {

        }
        public AddDayFormModel(ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
        }
        public AddDayFormModel(DateTime selectedDate, ImeHubDbContext db, IIdentity identity, DateTime now) : this(db, identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(selectedDate, db, PhysicianId.Value, now);
        }

        public Guid? CompanyId { get; set; }
        public Guid? AddressId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;
            public ViewDataModel(DateTime selectedDate, ImeHubDbContext db, Guid physicianId, DateTime now)
            {
                this.db = db;
                SelectedMonth = selectedDate.ToOrvosiDateFormat();
                LastDayOfMonth = selectedDate.LastDayOfMonth().ToOrvosiDateFormat();

                this.physicianId = physicianId;
                var physician = PhysicianModel.FromPhysician.Invoke(db.Physicians.Single(a => a.Id == physicianId));
                Physician = new PhysicianViewModel(physician);

                var availableDays = db.AvailableDays.Where(c => c.PhysicianId == physicianId).ToList();
                var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray();
                AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));

                Companies = GetPhysicianCompanySelectList();
                Addresses = GetPhysicianAddressSelectList();
            }
            public PhysicianViewModel Physician { get; set; }
            public string SelectedMonth { get; set; }
            public string LastDayOfMonth { get; }
            public MvcHtmlString AvailableDaysCSV { get; set; }
            public IEnumerable<SelectListItem> Companies { get; set; }
            public IEnumerable<SelectListItem> Addresses { get; set; }

            private List<SelectListItem> GetPhysicianCompanySelectList()
            {
                var companies = db.Companies
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(CompanyModel.FromCompany)
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
            private IEnumerable<SelectListItem> GetPhysicianAddressSelectList()
            {

                var physicianAddresses = db.Addresses
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(a => a.PhysicianId == physicianId)
                    .Select(AddressModel.FromAddress)
                    .ToList()
                    .Select(AddressViewModel.FromAddressModel);

                var companyIds = db.Companies
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(c => c.PhysicianId == physicianId)
                    .Select(c => c.Id)
                    .ToArray();

                var companyAddresses = db.Addresses
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(a => a.CompanyId.HasValue)
                    .Where(p => companyIds.Contains(p.CompanyId.Value))
                    .Select(AddressModel.FromAddress)
                    .ToList()
                    .Select(AddressViewModel.FromAddressModel);

                var addresses = physicianAddresses.Concat(companyAddresses);
                
                var list = addresses
                    .Select(d => new SelectListItem
                    {
                        Text = $"{d.Name} - {d.City}, {d.Address1}",
                        Value = d.Id.ToString(),
                        Group = new SelectListGroup { Name = d.Owner }
                    })
                    .OrderBy(d => d.Group.Name)
                    .ThenBy(d => d.Text);

                return list;
            }
        }
    }
}