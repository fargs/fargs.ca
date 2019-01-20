using LinqKit;
using ImeHub.Data;
using Enums = ImeHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ImeHub.Models
{
    public class CompanyModel : LookupModel<Guid>
    {
        public string Description { get; set; }
        public string BillingEmail { get; set; }
        public string ReportsEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int NoShowRate { get; set; }
        public Enums.RateFormat NoShowRateFormat { get; set; }
        public int LateCancellationRate { get; set; }
        public Enums.RateFormat LateCancellationRateFormat { get; set; }
        public int LateCancellationPolicy { get; set; }
        public IEnumerable<AddressModel> Addresses { get; set; }
        public IEnumerable<ServiceModel> Services { get; set; }

        public new static Expression<Func<Company, CompanyModel>> FromCompany = c => new CompanyModel
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
            NoShowRateFormat = (Enums.RateFormat)c.NoShowRateFormat,
            LateCancellationRate = c.LateCancellationRate,
            LateCancellationRateFormat = (Enums.RateFormat)c.LateCancellationRateFormat,
            LateCancellationPolicy = c.LateCancellationPolicy,
            Addresses = c.Addresses.AsQueryable().Select(AddressModel.FromAddress.Expand()),
            Services = c.Services.AsQueryable().Select(ServiceModel.FromServiceEntity.Expand())
        };
    }
}