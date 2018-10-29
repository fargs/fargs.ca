using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class AddressV2Dto
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? PhysicianId { get; set; }
        public byte AddressTypeId { get; set; }
        public string Name { get; set; }
        public string Attention { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public short CityId { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public short ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceCode { get; set; }
        public short TimeZoneId { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneIana { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public CompanyV2Dto Company { get; set; }
        public Physician Physician { get; set; }
        public LookupDto<byte> AddressType { get; set; }

        public override string ToString()
        {
            return $"{Address1}, {CityName} {ProvinceCode}, {Name}";
        }
        public static Expression<Func<AddressV2, AddressV2Dto>> FromAddressV2Entity = e => e == null ? null : new AddressV2Dto
        {
            Id = e.Id,
            CompanyId = e.CompanyId,
            PhysicianId = e.PhysicianId,
            AddressTypeId = e.AddressTypeId,
            Name = e.Name,
            Attention = e.Attention,
            Address1 = e.Address1,
            Address2 = e.Address2,
            CityId = e.CityId,
            CityName = e.City.Name,
            CityCode = e.City.Code,
            PostalCode = e.PostalCode,
            ProvinceCode = e.Province.ProvinceCode,
            TimeZone = e.TimeZone.Name,
            ProvinceName = e.Province.ProvinceName,
            CountryId = e.CountryId,
            CountryName = e.Country.Name,
            AddressType = LookupDto<byte>.FromAddressTypeEntity.Invoke(e.AddressType)
        };
    }
}