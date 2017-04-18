using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;

namespace WebApp.Models
{
    public class ProvinceDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public IEnumerable<LookupDto<short>> Cities { get; set; }

        public static Expression<Func<Province, ProvinceDto>> FromProvinceEntity = c => new ProvinceDto
        {
            Id = c.Id,
            Name = c.ProvinceName,
            Code = c.ProvinceCode,
            Cities = c.Cities.AsQueryable().Select(LookupDto<short>.FromCityEntity.Expand())
        };
    }
}