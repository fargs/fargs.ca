using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.ViewModels.PhysicianCompanyViewModels;

namespace WebApp.Library.Projections
{
    public class PhysicianCompanyProjections
    {
        public static Expression<Func<Orvosi.Data.PhysicianCompany, Company>> Basic()
        {
            return c => new Company
            {
                Id = c.Id,
                CompanyId = c.CompanyId,
                StatusId = c.StatusId,
                Status = c.PhysicianCompanyStatu.Name,
                Name = c.Company.Name,
                Code = c.Company.Code,
                BillingEmail = c.Company.BillingEmail,
                Phone = c.Company.Phone,
                AvailabilityCount = c.Company.AvailableDays.Count(),
                ServiceRequestCount = c.Company.ServiceRequests.Count(),
                ParentCompanyId = c.Company.Parent == null ? (short)0 : c.Company.Parent.Id,
                ParentCompanyName = c.Company.Parent == null ? string.Empty : c.Company.Parent.Name
            };
        }
    }
}