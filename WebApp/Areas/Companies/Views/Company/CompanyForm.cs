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
using WebApp.ViewModels;
using WebApp.Views.Shared;

namespace WebApp.Areas.Companies.Views.Company
{
    public class CompanyForm
    {
        public CompanyForm() { }
        public CompanyForm(Guid physicianId)
        {
            PhysicianId = physicianId;
        }
        public CompanyForm(Guid companyId, Guid physicianId, ImeHubDbContext db) : this(physicianId)
        {
            var company = db.Companies
                .Single(s => s.Id == companyId);

            CompanyId = companyId;
            Name = company.Name;
            Description = company.Description;
            Code = company.Code;
            ColorCode = company.ColorCode;
            BillingEmail = company.BillingEmail;
            ReportsEmail = company.ReportsEmail;
            PhoneNumber = company.PhoneNumber;
        }

        public Guid? CompanyId { get; set; }
        [Required]
        public Guid PhysicianId { get; set; }
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
    }
}