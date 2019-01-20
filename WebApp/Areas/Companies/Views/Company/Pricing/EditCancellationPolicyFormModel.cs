using ImeHub.Data;
using ImeHub.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImeHub.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class EditCancellationPolicyFormModel
    {
        public EditCancellationPolicyFormModel()
        {

        }
        public EditCancellationPolicyFormModel(Guid companyId, ImeHubDbContext db)
        {
            var company = db.Companies.Single(c => c.Id == companyId);
            CompanyId = company.Id;
            NoShowRate = company.NoShowRate;
            NoShowRateFormat = (RateFormat)company.NoShowRateFormat;
            LateCancellationRate = company.LateCancellationRate;
            LateCancellationRateFormat = (RateFormat)company.LateCancellationRateFormat;
            LateCancellationPolicy = company.LateCancellationPolicy;
        }
        public EditCancellationPolicyFormModel(CompanyModel company)
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