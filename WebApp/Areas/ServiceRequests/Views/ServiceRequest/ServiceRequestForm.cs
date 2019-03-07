using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using WebApp.Views.Shared;

namespace WebApp.Areas.ServiceRequests.Views.ServiceRequest
{
    public class ServiceRequestForm
    {
        private ImeHubDbContext db;
        public ServiceRequestForm() { }
        public ServiceRequestForm(Guid physicianId)
        {
            PhysicianId = physicianId;
            ViewData = new ViewDataModel();
        }
        public ServiceRequestForm(Guid physicianId, ImeHubDbContext db)
        {
            PhysicianId = physicianId;
            ViewData = new ViewDataModel(db, this);
        }
        public ServiceRequestForm(ServiceRequestForm form, ImeHubDbContext db)
        {
            this.db = db;
            this.PhysicianId = form.PhysicianId;
            this.ClaimantName = form.ClaimantName;
            this.ViewData = new ViewDataModel(db, form);
        }

        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public Guid? CompanyId { get; set; }
        [Required]
        public Guid? ServiceId { get; set; }
        [Required]
        public string ClaimantName { get; set; }
        public Guid? ServiceRequestId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private ServiceRequestForm form;

            public IEnumerable<LookupViewModel<Guid>> Companies { get; }
            public IEnumerable<CompanyViewModel> SelectedCompany { get; }
            public IEnumerable<LookupViewModel<Guid>> Services { get; }
            public IEnumerable<ServiceViewModel> SelectedService { get; }
            public IEnumerable<ClaimantViewModel> Claimants { get; }

            public ViewDataModel()
            {
                Claimants = new List<ClaimantViewModel>();
                Companies = new List<CompanyViewModel>();
                Services = new List<ServiceViewModel>();
            }
            public ViewDataModel(ImeHubDbContext db, ServiceRequestForm form) : this()
            {
                this.db = db;
                this.form = form;

                // always load the physician's companies
                Companies = GetCompanies();

                // always load the physician's claimants
                Claimants = GetClaimantSelectList();
                
                if (form.CompanyId.HasValue)
                {
                    Services = GetServices();
                }
            }

            private IEnumerable<CompanyViewModel> GetCompanies()
            {
                return db.Companies
                    .Where(c => c.PhysicianId == form.PhysicianId)
                    .Select(c => new CompanyViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code,
                        ColorCode = c.ColorCode
                    })
                    .OrderBy(c => c.Name)
                    .ToList();
            }
            private IEnumerable<ServiceViewModel> GetServices()
            {
                return db.Services
                    .Where(c => c.CompanyId == form.CompanyId)
                    .Select(c => new ServiceViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code,
                        ColorCode = c.ColorCode
                    })
                    .OrderBy(c => c.Name)
                    .ToList();
            }
            private IEnumerable<ClaimantViewModel> GetClaimantSelectList()
            {
                return db.ServiceRequests
                    .Where(sr => sr.PhysicianId == form.PhysicianId)
                    .Select(sr => new ClaimantViewModel
                    {
                        Id = sr.Id,
                        ClaimantName = sr.ClaimantName,
                        CompanyName = sr.Service.Company.Name
                    })
                    .AsEnumerable();
            }

            public class ClaimantViewModel
            {
                public Guid Id { get; set; }
                public string ClaimantName { get; set; }
                public string CompanyName { get; set; }
            }

            public class CompanyViewModel : LookupViewModel<Guid>
            {
            }
            public class ServiceViewModel : LookupViewModel<Guid>
            {

            }
        }
    }
}