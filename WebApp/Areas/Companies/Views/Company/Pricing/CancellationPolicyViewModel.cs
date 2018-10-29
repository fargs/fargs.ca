using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class CancellationPolicyViewModel
    {
        public CancellationPolicyViewModel(CancellationPolicyDto policy)
        {
            CompanyId = policy.CompanyId;
            NoShowRate = policy.NoShowRateFormat == RateFormat.Amount ? policy.NoShowRate.ToString("C2") : String.Format("{0}%", policy.NoShowRate.ToString());
            NoShowRateFormat = policy.NoShowRateFormat;
            LateCancellationRate = policy.LateCancellationRateFormat == RateFormat.Amount ? policy.LateCancellationRate.ToString("C2") : String.Format("{0}%", policy.LateCancellationRate.ToString());
            LateCancellationRateFormat = policy.LateCancellationRateFormat;
            LateCancellationPolicy = policy.LateCancellationPolicy;
        }
        public CancellationPolicyViewModel(CompanyV2Dto company)
        {
            CompanyId = company.Id;
            NoShowRate = company.NoShowRateFormat == RateFormat.Amount ? company.NoShowRate.ToString("C2") : String.Format("{0}%", company.NoShowRate.ToString());
            NoShowRateFormat = company.NoShowRateFormat;
            LateCancellationRate = company.LateCancellationRateFormat == RateFormat.Amount ? company.LateCancellationRate.ToString("C2") : String.Format("{0}%", company.LateCancellationRate.ToString());
            LateCancellationRateFormat = company.LateCancellationRateFormat;
            LateCancellationPolicy = company.LateCancellationPolicy;
        }
        public Guid CompanyId { get; set; }
        public string NoShowRate { get; set; }
        public RateFormat NoShowRateFormat { get; set; }
        public string LateCancellationRate { get; set; }
        public RateFormat LateCancellationRateFormat { get; set; }
        public int LateCancellationPolicy { get; set; }
    }
}