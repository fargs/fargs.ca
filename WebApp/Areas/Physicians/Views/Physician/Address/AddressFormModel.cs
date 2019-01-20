using LinqKit;
using ImeHub.Data;
using Enums = ImeHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Physicians.Views.Physician.Address
{
    public class AddressFormModel
    {
        public AddressFormModel()
        {
        }
        public AddressFormModel(AddressModel address, ImeHubDbContext db)
        {
            Id = address.Id;
            AddressTypeId = (byte)Enums.AddressType.CompanyAssessmentOffice;
            PhysicianId = address.PhysicianId.Value;
            Name = address.Name;
            Attention = address.Attention;
            Address1 = address.Address1;
            Address2 = address.Address2;
            PostalCode = address.PostalCode;
            ProvinceId = address.ProvinceId;
            CountryId = address.CountryId;
            TimeZoneId = address.TimeZoneId;
            IsBillingAddress = address.IsBillingAddress;
            ViewData = new ViewDataModel(db, address.PhysicianId.Value);
        }
        public AddressFormModel(Guid physicianId, ImeHubDbContext db)
        {
            PhysicianId = physicianId;
            AddressTypeId = (byte)Enums.AddressType.CompanyAssessmentOffice;
            CountryId = 1;
            ProvinceId = 9;
            TimeZoneId = 1;
            IsBillingAddress = true;
            ViewData = new ViewDataModel(db, physicianId);
        }
        public Guid? Id { get; set; }
        public byte AddressTypeId { get; set; }
        public Guid PhysicianId { get; set; }
        [Display(Name = "Building Name")]
        public string Name { get; set; }
        public string Attention { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public short ProvinceId { get; set; }
        public short TimeZoneId { get; set; }
        public int CountryId { get; set; }
        public bool IsBillingAddress { get; set; }
        public string IsBillingAddressChecked
        {
            get
            {
                return IsBillingAddress ? "checked" : "";
            }
        }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;

            public IEnumerable<SelectListItem> Countries { get; }
            public IEnumerable<SelectListItem> Provinces { get; }
            public IEnumerable<SelectListItem> Cities { get; }
            public IEnumerable<SelectListItem> AddressTypes { get; }
            public IEnumerable<SelectListItem> TimeZones { get; }

            public ViewDataModel(ImeHubDbContext db, Guid physicianId)
            {
                this.db = db;
                Countries = GetCountrySelectList();
                Provinces = GetProvinceSelectList();
                Cities = GetCitySelectList();
                AddressTypes = GetAddressTypeSelectList();
                TimeZones = GetTimeZoneSelectList();
            }

            private IEnumerable<SelectListItem> GetTimeZoneSelectList()
            {
                return db.TimeZones
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Iso
                    })
                    .AsEnumerable()
                    .Select(c => new SelectListItem()
                    {
                        Text = $"{c.Iso} ({c.Name})",
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }

            private IEnumerable<SelectListItem> GetAddressTypeSelectList()
            {
                return db.AddressTypes
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }

            private IEnumerable<SelectListItem> GetCitySelectList()
            {
                return db.Cities
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name + " - " + c.Province.ProvinceName,
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }

            private IEnumerable<SelectListItem> GetCountrySelectList()
            {
                return db.Countries
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }

            private IEnumerable<SelectListItem> GetProvinceSelectList()
            {
                return db.Provinces
                    .Select(c => new SelectListItem()
                    {
                        Text = c.ProvinceName,
                        Value = c.Id.ToString()
                    })
                    .OrderBy(c => c.Text)
                    .ToList();
            }
        }
    }
}