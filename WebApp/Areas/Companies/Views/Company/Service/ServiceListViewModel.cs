using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ServiceListViewModel
    {
        public ServiceListViewModel(CompanyV2Dto company)
        {
            CompanyId = company.Id;
            Services = company.Services.Where(s => !s.IsTravelRequired).Select(s => new ServiceViewModel(s));
            CancellationPolicy = new CancellationPolicyViewModel(company);
        }
        public Guid? CompanyId { get; set; }
        
        public IEnumerable<ServiceViewModel> Services { get; set; }
        public CancellationPolicyViewModel CancellationPolicy { get; set; }

    }
}