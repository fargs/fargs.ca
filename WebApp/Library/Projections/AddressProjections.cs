using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class AddressProjections
    {
        public static Expression<Func<Orvosi.Data.Address, Orvosi.Shared.Model.Address>> MinimalInfo()
        {
            return sr => new Orvosi.Shared.Model.Address
            {
                Id = sr.Id,
                Name = sr.Name,
                City = sr.City_CityId.Name,
                CityCode = sr.City_CityId.Code,
                PostalCode = sr.PostalCode,
                Address1 = sr.Address1,
                ProvinceCode = sr.Province.ProvinceCode,
                TimeZone = sr.TimeZone.Name
            };
        }

        public static Expression<Func<Orvosi.Data.Address, AddressResult>> Search()
        {
            return sr => new AddressResult
            {
                Id = sr.Id,
                OwnerGuid = sr.OwnerGuid,
                Name = sr.Name,
                City = sr.City_CityId.Name,
                CityCode = sr.City_CityId.Code,
                PostalCode = sr.PostalCode,
                Address1 = sr.Address1,
                ProvinceCode = sr.Province.ProvinceCode,
                TimeZone = sr.TimeZone.Name
            };
        }

        public class AddressResult
        {
            public int Id { get; set; }
            public Guid? OwnerGuid { get; set; }
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string CityCode { get; set; }
            public string ProvinceCode { get; set; }
            public string TimeZone { get; set; }
            public short? ProvinceId { get; set; }
            public string TimeZoneIana { get; set; }
        }
    }
}