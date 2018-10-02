using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Views.Address
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string ProvinceCode { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneIana { get; set; }

        public static Expression<Func<AddressDto, AddressViewModel>> FromAddressDto = dto => dto == null ? null : new AddressViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            City = dto.City,
            CityCode = dto.CityCode,
            PostalCode = dto.PostalCode,
            Address1 = dto.Address1,
            ProvinceCode = dto.ProvinceCode,
            TimeZone = dto.TimeZone
        };
    }
}