using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;
using Enums = Orvosi.Shared.Enums;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AddDayFormModel : ViewModelBase
    {
        public AddDayFormModel()
        {

        }
        public AddDayFormModel(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
        }
        public AddDayFormModel(DateTime selectedDate, OrvosiDbContext db, IIdentity identity, DateTime now) : this(db, identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(selectedDate, db, PhysicianId.Value, now);
        }

        public short? CompanyId { get; set; }
        public int? LocationId { get; set; }
        public bool IsPrebook { get; set; } = false;

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private OrvosiDbContext db;
            private Guid physicianId;
            public ViewDataModel(DateTime selectedDate, OrvosiDbContext db, Guid physicianId, DateTime now)
            {
                this.db = db;
                SelectedMonth = selectedDate.ToOrvosiDateFormat();
                LastDayOfMonth = selectedDate.LastDayOfMonth().ToOrvosiDateFormat();

                this.physicianId = physicianId;
                var physician = PersonDto.FromAspNetUserEntity.Invoke(db.AspNetUsers.Single(a => a.Id == physicianId));
                Physician = LookupViewModel<Guid>.FromPersonDto(physician);

                var availableDays = db.AvailableDays.Where(c => c.PhysicianId == physicianId).ToList();
                var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray();
                AvailableDaysCSV = MvcHtmlString.Create(string.Join(",", arr));

                Companies = GetPhysicianCompanySelectList();
                Addresses = GetPhysicianAddressSelectList();
            }
            public LookupViewModel<Guid> Physician { get; set; }
            public string SelectedMonth { get; set; }
            public string LastDayOfMonth { get; }
            public MvcHtmlString AvailableDaysCSV { get; set; }
            public IEnumerable<SelectListItem> Companies { get; set; }
            public IEnumerable<SelectListItem> Addresses { get; set; }

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
            private IEnumerable<SelectListItem> GetPhysicianAddressSelectList()
            {
                var physician = db.AspNetUsers
                    .Where(a => a.Id == physicianId)
                    .Select(PersonDto.FromAspNetUserEntity.Expand())
                    .ToList()
                    .Select(LookupViewModel<Guid>.FromPersonDto);

                var companies = db.PhysicianCompanies
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(c => new LookupDto<Guid>
                    {
                        Id = c.Company.ObjectGuid.Value,
                        Name = c.Company.Name
                    })
                    .ToList()
                    .Select(LookupViewModel<Guid>.FromLookupDto);

                var entities = physician.Concat(companies);
                var ownerIds = entities.Select(e => e.Id).ToArray();

                var addresses = db.Addresses
                    .Where(a => a.OwnerGuid.HasValue)
                    .Where(a => ownerIds.Contains(a.OwnerGuid.Value))
                    .Where(a => a.AddressTypeId != Enums.AddressTypes.BillingAddress)
                    .Select(AddressDto.FromAddressEntity.Expand())
                    .ToList();

                // join with addresses to get the owners
                var addressesWithOwners = addresses
                    .Join(entities,
                        a => a.OwnerGuid.Value,
                        e => e.Id,
                        (a, e) => new
                        {
                            Address = a,
                            Owner = e
                        });

                var query = addressesWithOwners
                    .Select(d => new SelectListItem
                    {
                        Text = $"{d.Address.Name} - {d.Address.City}, {d.Address.Address1}",
                        Value = d.Address.Id.ToString(),
                        Group = new SelectListGroup { Name = d.Owner == null ? string.Empty : d.Owner.Name }
                    })
                    .OrderBy(d => d.Group.Name)
                    .ThenBy(d => d.Text);

                var noOwners = query
                    .Where(d => string.IsNullOrEmpty(d.Group.Name))
                    .OrderBy(d => d.Text);
                var owners = query
                    .Where(d => !string.IsNullOrEmpty(d.Group.Name))
                    .OrderBy(d => d.Group.Name)
                    .ThenBy(d => d.Text);

                return owners.Concat(noOwners);
            }
        }
    }
}