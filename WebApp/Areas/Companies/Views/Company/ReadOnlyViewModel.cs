using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class ReadOnlyViewModel : ViewModelBase
    {
        public ReadOnlyViewModel() { }
        public ReadOnlyViewModel(IIdentity identity, DateTime now) : base(identity, now)
        {
            PhysicianId = PhysicianId;
        }
        public ReadOnlyViewModel(Guid companyId, ImeHubDbContext db, IIdentity identity, DateTime now) : this(identity, now)
        {
            var company = db.Companies
                .AsNoTracking()
                .AsExpandable()
                .Select(CompanyModel.FromCompany)
                .SingleOrDefault(s => s.Id == companyId);
            
            CompanyId = companyId;
            Name = company.Name;
            Description = company.Description;
            Code = company.Code;
            ColorCode = company.ColorCode;
            BillingEmail = company.BillingEmail;
            ReportsEmail = company.ReportsEmail;
            PhoneNumber = company.PhoneNumber;
            AddressList = new AddressListViewModel(company);
            ServiceList = new ServiceListViewModel(company);
            PricingMatrix = new PricingMatrixViewModel(db, company, PhysicianId.Value);
        }

        public Guid? CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [StringLength(2)]
        public string Code { get; set; }
        [Required]
        public string ColorCode { get; set; }
        public string BillingEmail { get; set; }
        public string ReportsEmail { get; set; }
        public string PhoneNumber { get; set; }
        public AddressListViewModel AddressList { get; set; }
        public ServiceListViewModel ServiceList { get; set; }
        public PricingMatrixViewModel PricingMatrix { get; set; }
    }
}