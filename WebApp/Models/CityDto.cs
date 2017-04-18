using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;

namespace WebApp.Models
{
    public class CityDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public LookupDto<short> Province { get; set; }

        public static Expression<Func<City, CityDto>> FromCityEntity = c => new CityDto
        {
            Id = c.Id,
            Name = c.Name,
            ProvinceId = c.ProvinceId,
            Province = LookupDto<short>.FromProvinceEntity.Invoke(c.Province)
        };
    }
}