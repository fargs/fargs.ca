using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class AddressFormModel
    {
        public AddressFormModel()
        {
        }
        public AddressFormModel(AddressV2Dto address, Guid physicianId, OrvosiDbContext db)
        {
            Id = address.Id;
            AddressTypeId = address.AddressTypeId;
            CompanyId = address.CompanyId.Value;
            Name = address.Name;
            Attention = address.Attention;
            Address1 = address.Address1;
            Address2 = address.Address2;
            PostalCode = address.PostalCode;
            CityId = address.CityId;
            ProvinceId = address.ProvinceId;
            CountryId = address.CountryId;
            TimeZoneId = address.TimeZoneId;
            ViewData = new ViewDataModel(db, physicianId);
        }
        public AddressFormModel(Guid companyId, Guid physicianId, OrvosiDbContext db)
        {
            CompanyId = companyId;
            AddressTypeId = 4;
            CountryId = 124;
            ProvinceId = 9;
            TimeZoneId = 1;
            ViewData = new ViewDataModel(db, physicianId);
        }
        public Guid? Id { get; set; }
        public byte AddressTypeId { get; set; }
        public Guid CompanyId { get; set; }
        [Display(Name = "Building Name")]
        public string Name { get; set; }
        public string Attention { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public short CityId { get; set; }
        public short ProvinceId { get; set; }
        public short TimeZoneId { get; set; }
        public int CountryId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private OrvosiDbContext db;

            public IEnumerable<SelectListItem> Countries { get; }
            public IEnumerable<SelectListItem> Provinces { get; }
            public IEnumerable<SelectListItem> Cities { get; }
            public IEnumerable<SelectListItem> AddressTypes { get; }
            public IEnumerable<SelectListItem> TimeZones { get; }

            public ViewDataModel(OrvosiDbContext db, Guid physicianId)
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
                    .Where(c => c.Id == 124) // Canada
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
                    .Where(c => c.CountryId == 124) // Canada
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