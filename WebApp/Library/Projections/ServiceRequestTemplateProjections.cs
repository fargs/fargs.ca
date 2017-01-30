using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class ServiceRequestTemplateProjections
    {
        public static Expression<Func<Orvosi.Data.ServiceRequestTemplate, ServiceRequestTemplateResult>> Search()
        {
            return c => new ServiceRequestTemplateResult
            {
                id = c.Id,
                Id = c.Id,
                Name = c.Name
            };
        }
        public class ServiceRequestTemplateResult
        {
            public short id { get; set; }
            public short Id { get; set; }
            public string Name { get; set; }
        }
    }
}