using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class EditCancellationPolicyFormModel
    {
        public EditCancellationPolicyFormModel()
        {

        }
        public EditCancellationPolicyFormModel(Guid companyId, OrvosiDbContext db)
        {
            var company = db.CompanyV2.Single(c => c.Id == companyId);
            CompanyId = company.Id;
            NoShowRate = company.NoShowRate;
            NoShowRateFormat = (RateFormat)company.NoShowRateFormat;
            LateCancellationRate = company.LateCancellationRate;
            LateCancellationRateFormat = (RateFormat)company.LateCancellationRateFormat;
            LateCancellationPolicy = company.LateCancellationPolicy;
        }
        public EditCancellationPolicyFormModel(CompanyV2Dto company)
        {
            CompanyId = company.Id;
            NoShowRate = company.NoShowRate;
            NoShowRateFormat = company.NoShowRateFormat;
            LateCancellationRate = company.LateCancellationRate;
            LateCancellationRateFormat = company.LateCancellationRateFormat;
            LateCancellationPolicy = company.LateCancellationPolicy;
        }
        public Guid CompanyId { get; set; }
        public int NoShowRate { get; set; }
        public RateFormat NoShowRateFormat { get; set; }
        public int LateCancellationRate { get; set; }
        public RateFormat LateCancellationRateFormat { get; set; }
        public int LateCancellationPolicy { get; set; }
    }
}