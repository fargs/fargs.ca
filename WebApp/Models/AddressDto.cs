using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string ProvinceCode { get; set; }
        public string TimeZone { get; set; }
        public short? ProvinceId { get; set; }
        public string TimeZoneIana { get; set; }

        public override string ToString()
        {
            return $"{Address1}, {City} {ProvinceCode}, {Name}";
        }

        public static Expression<Func<Address, AddressDto>> FromAddressEntity = e => e == null ? null : new AddressDto
        {
            Id = e.Id,
            Name = e.Name,
            City = e.City_CityId.Name,
            CityCode = e.City_CityId.Code,
            PostalCode = e.PostalCode,
            Address1 = e.Address1,
            ProvinceCode = e.Province.ProvinceCode,
            TimeZone = e.TimeZone.Name
        };
    }
}