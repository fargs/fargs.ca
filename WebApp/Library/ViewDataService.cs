using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.Projections;

namespace WebApp.Library
{
    public class ViewDataService
    {
        IOrvosiDbContext dbContext;
        IIdentity identity;
        Guid userId;

        public ViewDataService(IIdentity identity, IOrvosiDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.identity = identity;
            this.userId = identity.GetGuidUserId();
        }

        public List<Orvosi.Shared.Model.Person> GetPhysicians()
        {
            var physicians = identity.GetPhysicians();

            return dbContext.AspNetUsers
                .Where(u => physicians.Contains(u.Id))
                .Select(AspNetUserProjections.Basic())
                .ToList();
        }

        public List<SelectListItem> GetPhysicianSelectList()
        {
            var data = GetPhysicians();

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.DisplayName,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

        public List<CompanyProjections.CompanySearchResult> GetPhysicianCompanies(Guid? physicianId)
        {
            return dbContext.PhysicianCompanies
                            .Where(p => p.PhysicianId == physicianId)
                            .Select(c => c.Company)
                            .Select(CompanyProjections.Search())
                            .ToList();
        }

        public List<SelectListItem> GetPhysicianCompanySelectList(Guid? physicianId)
        {
            List<CompanyProjections.CompanySearchResult> data = GetPhysicianCompanies(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.id.ToString()
                })
                .ToList();
        }

        public List<SelectListItem> GetPhysicianCompanySelectListWithGuid(Guid? physicianId)
        {
            List<CompanyProjections.CompanySearchResult> data = GetPhysicianCompanies(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.ObjectGuid.ToString()
                })
                .ToList();
        }

        public List<ServiceProjections.ServiceResult> GetPhysicianServices(Guid? physicianId)
        {
            return dbContext.PhysicianServices
                            .Where(p => p.PhysicianId == physicianId)
                            .Select(c => c.Service)
                            .Select(ServiceProjections.Search())
                            .ToList();
        }

        public List<SelectListItem> GetPhysicianServiceSelectList(Guid? physicianId)
        {
            var data = GetPhysicianServices(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.id.ToString()
                })
                .ToList();
        }

        public List<Orvosi.Shared.Model.Person> GetPhysicianCaseCoordinators(Guid? physicianId)
        {
            return dbContext.Collaborators
                            .Where(c => c.UserId == physicianId)
                            .Select(c => c.CollaboratorUser)
                            .Where(c => c.AspNetUserRoles.Select(r => r.RoleId).FirstOrDefault() == Orvosi.Shared.Enums.AspNetRoles.CaseCoordinator)
                            .Select(AspNetUserProjections.Basic())
                            .ToList();
        }

        public List<SelectListItem> GetPhysicianCaseCoordinatorSelectList(Guid? physicianId)
        {
            var data = GetPhysicianCaseCoordinators(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.DisplayName,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

        public List<ServiceRequestTemplateProjections.ServiceRequestTemplateResult> GetPhysicianProcessTemplates(Guid? physicianId)
        {
            return dbContext.ServiceRequestTemplates
                            .Where(c => c.PhysicianId == physicianId)
                            .Select(ServiceRequestTemplateProjections.Search())
                            .ToList();
        }

        public List<SelectListItem> GetPhysicianProcessTemplateSelectList(Guid? physicianId)
        {
            var data = GetPhysicianProcessTemplates(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

        public List<ServiceRequestTemplateProjections.ServiceRequestTemplateResult> GetDefaultProcessTemplates()
        {
            return dbContext.ServiceRequestTemplates
                            .Where(c => c.IsDefault)
                            .Select(ServiceRequestTemplateProjections.Search())
                            .ToList();
        }

        public List<SelectListItem> GetDefaultProcessTemplateSelectList()
        {
            var data = GetDefaultProcessTemplates();

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

        public List<AddressViewModel> GetPhysicianAddresses(Guid? physicianId)
        {
            var query1 = dbContext.AspNetUsers
                .Where(a => a.Id == physicianId)
                .Select(AspNetUserProjections.Basic())
                .ToList()
                .Select(a => new OwnerViewModel { Id = a.Id, Name = a.DisplayName });

            // retrieve all the companies from the database
            var query2 = GetPhysicianCompanies(physicianId)
                .Select(c => new OwnerViewModel { Id = c.ObjectGuid, Name = c.Name })
                .ToList();

            // concat them into 1 list
            var entities = query1.Concat(query2);

            var ownerIds = entities.Select(e => e.Id).ToArray();
            var addresses = dbContext.Addresses
                .Where(a => ownerIds.Contains(a.OwnerGuid))
                .Where(a => a.AddressTypeId != AddressTypes.BillingAddress)
                .Select(AddressProjections.Search())
                .ToList();

            // join with addresses to get the owners
            return addresses
                .Join(entities, 
                    a => a.OwnerGuid, 
                    e => e.Id,
                    (a, e) => new AddressViewModel
                    {
                        Address = a,
                        Owner = e
                    })
                   .ToList();
        }

        public List<SelectListItem> GetPhysicianAddressSelectList(Guid? physicianId)
        {
            var data = GetPhysicianAddresses(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = $"{d.Address.City}, {d.Address.Address1}",
                    Value = d.Address.Id.ToString(),
                    Group = new SelectListGroup { Name = d.Owner == null ? string.Empty : d.Owner.Name }
                })
                .ToList();
        }
        
        public class AddressViewModel
        {
            public AddressProjections.AddressResult Address { get; set; }
            public OwnerViewModel Owner { get; set; }
        }

        public List<Orvosi.Shared.Model.Person> GetCollaborations(Guid physicianId)
        {
            return dbContext.Collaborators
                .Where(ww => ww.UserId == physicianId)
                .Select(CollaboratorProjections.Basic())
                .ToList();
        }

        public List<SelectListItem> GetCollaborationSelectList(Guid physicianId)
        {
            var data = GetCollaborations(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.DisplayName,
                    Value = d.Id.ToString(),
                    Group = new SelectListGroup { Name = d.Role.Name }
                })
                .ToList();
        }

    }
}