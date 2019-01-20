using ImeHub.Data;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace ImeHub.Models
{
    public class AddressModel
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
        public Guid CityId { get; set; }
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

        public LookupModel<Guid> Company { get; set; }
        public LookupModel<Guid> Physician { get; set; }
        public LookupModel<byte> AddressType { get; set; }
        public bool IsBillingAddress { get; set; }

        public override string ToString()
        {
            return $"{Address1}, {CityName} {ProvinceCode}, {Name}";
        }
        public static Expression<Func<Address, AddressModel>> FromAddress = e => e == null ? null : new AddressModel
        {
            Id = e.Id,
            CompanyId = e.CompanyId,
            Company = !e.CompanyId.HasValue ? null : new LookupModel<Guid>
            {
                Id = e.Company.Id,
                Name = e.Company.Name,
                Code = e.Company.Code,
                ColorCode = e.Company.ColorCode
            },
            PhysicianId = e.PhysicianId,
            Physician = !e.PhysicianId.HasValue ? null : new LookupModel<Guid>
            {
                Id = e.Physician.Id,
                Name = e.Physician.CompanyName,
                Code = e.Physician.Code,
                ColorCode = e.Physician.ColorCode
            },
            AddressTypeId = e.AddressTypeId,
            Name = e.Name,
            Attention = e.Attention,
            Address1 = e.Address1,
            Address2 = e.Address2,
            CityId = e.CityId,
            CityName = e.City.Name,
            CityCode = e.City.Code,
            PostalCode = e.PostalCode,
            ProvinceCode = e.City.Province.ProvinceCode,
            TimeZone = e.TimeZone.Name,
            ProvinceName = e.City.Province.ProvinceName,
            CountryId = e.City.Province.CountryId,
            CountryName = e.City.Province.Country.Name,
            IsBillingAddress = e.IsBillingAddress,
            AddressType = new LookupModel<byte>
            {
                Id = e.AddressTypeId,
                Name = e.AddressType.Name,
                Code = null,
                ColorCode = null
            }
        };
    }
}