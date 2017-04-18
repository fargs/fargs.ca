using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;
using Orvosi.Shared.Enums;

namespace WebApp.Models
{
    public class ServiceCategoryDto
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<LookupDto<short>> Services { get; set; }

        public static Expression<Func<ServiceCategory, ServiceCategoryDto>> FromPhysicianServicePortfolioEntity = c => new ServiceCategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Services = c.Services
                .Where(s => s.ServicePortfolioId == ServicePortfolios.Physician)
                .AsQueryable()
                .Select(LookupDto<short>.FromServiceEntity.Expand())
        };
    }
}