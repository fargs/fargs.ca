using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Data;
using ImeHub.Models.Enums;

namespace ImeHub.Models
{
    public class CancellationPolicyModel
    {
        public Guid CompanyId { get; set; }
        public int NoShowRate { get; set; }
        public RateFormat NoShowRateFormat { get; set; }
        public int LateCancellationRate { get; set; }
        public RateFormat LateCancellationRateFormat { get; set; }
        public int LateCancellationPolicy { get; set; }

        public Expression<Func<Company, CancellationPolicyModel>> FromCompany = c => new CancellationPolicyModel
        {
            CompanyId = c.Id,
            NoShowRate = c.NoShowRate,
            NoShowRateFormat = (RateFormat)c.NoShowRateFormat,
            LateCancellationRate = c.LateCancellationRate,
            LateCancellationRateFormat = (RateFormat)c.LateCancellationRateFormat,
            LateCancellationPolicy = c.LateCancellationPolicy
        };
    }
}