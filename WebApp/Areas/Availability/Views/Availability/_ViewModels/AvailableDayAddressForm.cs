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
using WebApp.Views.Shared;
using Enums = Orvosi.Shared.Enums;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayAddressForm : ViewModelBase
    {
        public AvailableDayAddressForm()
        {

        }
        public AvailableDayAddressForm(OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue) throw new Exception("Physician context must be set.");
            ViewData = new ViewDataModel(db, PhysicianId.Value);
        }
        [Required]
        public short AvailableDayId { get; set; }
        public int? AddressId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private OrvosiDbContext db;
            public ViewDataModel(OrvosiDbContext db, Guid physicianId)
            {
                this.db = db;
                Addresses = GetPhysicianAddressSelectList(physicianId);
            }
            public IEnumerable<SelectListItem> Addresses { get; set; }

            private IEnumerable<SelectListItem> GetPhysicianAddressSelectList(Guid? physicianId)
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