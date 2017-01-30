using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Library.Projections
{
    public class CompanyProjections
    {
        public static Expression<Func<Orvosi.Data.Company, CompanySearchResult>> Search()
        {
            return c => new CompanySearchResult
            {
                id = c.Id,
                Id = c.Id,
                ObjectGuid = c.ObjectGuid,
                CompanyName = c.Name,
                ParentName = c.Parent == null ? string.Empty : c.Parent.Name
            };
        }
        public class CompanySearchResult
        {
            public short id { get; set; }
            public short Id { get; set; }
            public Guid? ObjectGuid { get; set; }
            public string CompanyName { get; set; }
            public string ParentName { get; set; }
            public string Name {
                get
                {
                    return string.IsNullOrEmpty(ParentName) ? CompanyName : $"{ParentName} - {CompanyName}";
                }
            }
        }
    }
}