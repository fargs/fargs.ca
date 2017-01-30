using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class ServiceProjections
    {
        public static Expression<Func<Orvosi.Data.Service, ServiceResult>> Search()
        {
            return c => new ServiceResult
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Catagory = c.ServiceCategory.Name,
                ColorCode = c.ColorCode
            };
        }

        public class ServiceResult
        {
            public short Id { get; set; }
            public string Name { get; set; }
            public string Catagory { get; set; }
            public string Code { get; set; }
            public string ColorCode { get; set; }

            public short id => Id;
            public string DisplayName => Name;
        }

        public class ListViewModel
        {
            public IEnumerable<ServiceResult> Services { get; set; }
            public int ServiceCount { get; set; }
        }
    }
}