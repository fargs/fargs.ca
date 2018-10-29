using LinqKit;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class CompanyV2Dto : LookupDto<Guid>
    {
        public string Description { get; set; }
        public string BillingEmail { get; set; }
        public string ReportsEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int NoShowRate { get; set; }
        public RateFormat NoShowRateFormat { get; set; }
        public int LateCancellationRate { get; set; }
        public RateFormat LateCancellationRateFormat { get; set; }
        public int LateCancellationPolicy { get; set; }
        public IEnumerable<AddressV2Dto> Addresses { get; set; }
        public IEnumerable<CompanyServiceDto> Services { get; set; }

        public static Expression<Func<CompanyV2, CompanyV2Dto>> FromCompanyV2Entity = c => new CompanyV2Dto
        {
            Id = c.Id,
            Name = c.Name,
            Code = c.Code,
            ColorCode = c.ColorCode,
            Description = c.Description,
            BillingEmail = c.BillingEmail,
            ReportsEmail = c.ReportsEmail,
            PhoneNumber = c.PhoneNumber,
            NoShowRate = c.NoShowRate,
            NoShowRateFormat = (RateFormat)c.NoShowRateFormat,
            LateCancellationRate = c.LateCancellationRate,
            LateCancellationRateFormat = (RateFormat)c.LateCancellationRateFormat,
            LateCancellationPolicy = c.LateCancellationPolicy,
            Addresses = c.AddressV2.AsQueryable().Select(AddressV2Dto.FromAddressV2Entity.Expand()),
            Services = c.CompanyServices.AsQueryable().Select(CompanyServiceDto.FromCompanyServiceEntity.Expand())
        };
    }
}