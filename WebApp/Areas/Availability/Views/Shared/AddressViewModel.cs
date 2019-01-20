using ImeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Areas.Availability.Views.Shared
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public string ProvinceCode { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneIana { get; set; }
        public string Owner { get; set; }

        public static Func<AddressModel, AddressViewModel> FromAddressModel = dto => dto == null ? null : new AddressViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            City = dto.CityName,
            CityCode = dto.CityCode,
            PostalCode = dto.PostalCode,
            Address1 = dto.Address1,
            ProvinceCode = dto.ProvinceCode,
            TimeZone = dto.TimeZone,
            Owner = dto.PhysicianId.HasValue ? dto.Physician.Name : dto.Company.Name
        };
        public static Expression<Func<AddressModel, AddressViewModel>> FromAddressDtoExpr = dto => FromAddressModel(dto);
    }
}