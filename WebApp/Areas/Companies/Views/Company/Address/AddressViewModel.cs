using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class AddressViewModel
    {
        public AddressViewModel(AddressModel address)
        {
            Id = address.Id;
            CompanyId = address.CompanyId;
            PhysicianId = address.PhysicianId;
            AddressType = address.AddressType.Name;
            Name = address.Name;
            Address1 = address.Address1;
            Address2 = address.Address2;
            CityId = address.CityId;
            CityName = address.CityName;
            CityCode = address.CityCode;
            PostalCode = address.PostalCode;
            ProvinceCode = address.ProvinceCode;
            TimeZone = address.TimeZone;
            ProvinceName = address.ProvinceName;
            CountryId = address.CountryId;
            CountryName = address.CountryName;
            IsBillingAddress = address.IsBillingAddress;
        }
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? PhysicianId { get; set; }
        public string AddressType { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public Guid CityId { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public short ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceCode { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneIana { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsBillingAddress { get; set; }

        public override string ToString()
        {
            return $"{Address1}, {CityName} {ProvinceCode}, {Name}";
        }
        
    }
}