using LinqKit;
using MoreLinq;
using Orvosi.Data;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.FormModels;
using WebApp.Library;
using WebApp.Library.Filters;
using WebApp.Models;
using WebApp.ViewModels;
using Features = Orvosi.Shared.Enums.Features;

namespace WebApp.Controllers
{
    public class ResourcesController : BaseController
    {
        private OrvosiDbContext db;
        private ViewDataService viewDataService;
        private WorkService service;

        public ResourcesController(OrvosiDbContext db, WorkService service, ViewDataService viewDataService, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.db = db;
            this.service = service;
            this.viewDataService = viewDataService;
        }
        [HttpGet]
        [AuthorizeRole(Feature = Features.ServiceRequest.View)]
        public async Task<ActionResult> List(int serviceRequestId)
        {
            var dto = db.ServiceRequestResources
                .Where(r => r.ServiceRequestId == serviceRequestId)
                .Select(ResourceDto.FromServiceRequestResourceEntity.Expand())
                .ToList();

            var viewModel = dto
                .AsQueryable()
                .Select(ResourceViewModel.FromResourceDto.Expand());

            return PartialView("_Resources", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> ShowResourceForm(int serviceRequestId)
        {
            var entity = await db.ServiceRequests.FindAsync(serviceRequestId);

            var form = new AdditionalResourceForm()
            {
                PhysicianId = entity.PhysicianId,
                ServiceRequestId = serviceRequestId
            };

            return PartialView("_ResourceModalForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> ShowRequiredResourcesForm(int serviceRequestId)
        {
            var entity = await db.ServiceRequests.FindAsync(serviceRequestId);
            var dto = ServiceRequestDto.FromServiceRequestEntity.Invoke(entity);
            
            var resourcesViewModel = dto.Resources
                .AsQueryable()
                .Where(r => r.RoleId != AspNetRoles.Physician)
                .Where(r => r.RoleId.HasValue)
                .Select(RequiredResourceForm.FromResourceDto.Expand());
            
            var viewModel = new RequiredResourcesForm()
            {
                PhysicianId = dto.Physician.Id,
                ServiceRequestTemplateId = dto.ServiceRequestTemplateId,
                ServiceRequestId = serviceRequestId,
                Resources = resourcesViewModel
            };

            return PartialView("_RequiredResourcesModalForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveResource(AdditionalResourceForm form)
        {
            if (ModelState.IsValid)
            {
                var resourceId = await service.CreateAdditionalResource(form);

                return Json(new
                {
                    serviceRequestId = form.ServiceRequestId,
                    serviceRequestResourceId = resourceId

                });
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            return PartialView("_ResourceModalForm", form);
        }

        [HttpPost]
        public async Task<ActionResult> SaveResources(RequiredResourcesForm forms)
        {
            var sr = await db.ServiceRequests.Include(s => s.ServiceRequestResources).FirstAsync(s => s.Id == forms.ServiceRequestId);
            if (ModelState.IsValid)
            {
                forms.Resources.ForEach(formItem => 
                    service.SaveRequiredResources(formItem, forms.ServiceRequestId, sr.ServiceRequestResources)
                );

                await db.SaveChangesAsync();
                
                return Json(new
                {
                    serviceRequestId = forms.ServiceRequestId
                });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return PartialView("_RequiredResourcesModalForm", forms);
        }

        [HttpGet]
        public async Task<ActionResult> ShowDeleteResourceForm(Guid resourceId)
        {
            var dto = await db.ServiceRequestResources.FindAsync(resourceId);
            var viewModel = new AdditionalResourceForm()
            {
                ResourceId = dto.Id,
                ServiceRequestId = dto.ServiceRequestId,
                UserId = dto.UserId
            };

            return PartialView("_DeleteResourceModalForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteResource(Guid resourceId)
        {
            var resource = await db.ServiceRequestResources.FindAsync(resourceId);
            await service.DeleteResource(resourceId);
            return Json(new
            {
                serviceRequestId = resource.ServiceRequestId,
                resourceId = resourceId
            });
        }
    }
}