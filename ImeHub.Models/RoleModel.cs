using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ImeHub.Models
{
    public class RoleModel : LookupModel<Guid>
    {
        public IEnumerable<FeatureModel> Features { get; set; }

        public static Expression<Func<Data.Role, RoleModel>> FromRole = e => e == null ? null : new RoleModel
        {
            Id = e.Id,
            Name = e.Name,
            Code = e.Code,
            ColorCode = e.ColorCode,
            Features = e.RoleFeatures.Select(rf => new FeatureModel
            {
                Id = rf.Feature.Id,
                Name = rf.Feature.Name
            })
        };
    }
}
