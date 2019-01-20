using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using WebApp.Views.Shared;
using Enums = ImeHub.Models.Enums;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayAddressForm : ViewModelBase
    {
        public AvailableDayAddressForm()
        {

        }
        public AvailableDayAddressForm(ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(db, PhysicianId.Value);
        }
        [Required]
        public Guid AvailableDayId { get; set; }
        public Guid? AddressId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                Addresses = GetPhysicianAddressSelectList(physicianId);
            }
            public IEnumerable<SelectListItem> Addresses { get; set; }

            private IEnumerable<SelectListItem> GetPhysicianAddressSelectList(Guid? physicianId)
            {
                var physician = db.Physicians
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(a => a.Id == physicianId)
                    .Select(p => new LookupModel<Guid>
                    {
                        Id = p.Id,
                        Name = p.CompanyName
                    })
                    .ToList()
                    .Select(LookupViewModel<Guid>.FromLookupModel);

                var companies = db.Companies
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(c => new LookupModel<Guid>
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToList()
                    .Select(LookupViewModel<Guid>.FromLookupModel);

                var entities = physician.Concat(companies);
                var ownerIds = entities.Select(e => e.Id).ToArray();

                var addresses = db.Addresses
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(a => ownerIds.Contains(a.PhysicianId.Value) || ownerIds.Contains(a.CompanyId.Value))
                    .Where(a => a.AddressTypeId != (byte)Enums.AddressType.Billing)
                    .Select(AddressModel.FromAddress)
                    .ToList();

                // join with addresses to get the owners
                var addressesWithOwners = addresses
                    .Join(entities,
                        a => a.PhysicianId.Value, // TODO: FIX THIS TO INCLUDE COMPANYID 
                        e => e.Id,
                        (a, e) => new
                        {
                            Address = a,
                            Owner = e
                        });

                var query = addressesWithOwners
                    .Select(d => new SelectListItem
                    {
                        Text = $"{d.Address.Name} - {d.Address.CityName}, {d.Address.Address1}",
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