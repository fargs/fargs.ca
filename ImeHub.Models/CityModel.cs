using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data = ImeHub.Data;

namespace ImeHub.Models
{
    public class CityModel : LookupModel<Guid>
    {
        public short ProvinceId { get; set; }

        public static Expression<Func<Data.City, CityModel>> FromCity = a => a == null ? null : new CityModel
        {
            Id = a.Id,
            Name = a.Name,
            Code = a.Code,
            ProvinceId = a.ProvinceId
        };
    }
}
