﻿using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Library.Projections;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Library
{
    public class ViewDataService
    {
        IOrvosiDbContext dbContext;
        IIdentity identity;
        Guid userId;

        public ViewDataService(OrvosiDbContext db, IPrincipal principal)
        {
            this.dbContext = db;
            this.identity = principal.Identity;
            this.userId = identity.GetGuidUserId();
        }

        public LookupViewModel<Guid> GetPhysician(int serviceRequestId)
        {
            var data = dbContext.ServiceRequests.Single(sr => sr.Id == serviceRequestId);

            var dto = ServiceRequestDto.FromServiceRequestEntity.Invoke(data);

            var viewModel = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.Physician);

            return viewModel;
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
                .OrderBy(d => d.Text)
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
                .OrderBy(d => d.Text)
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
                .OrderBy(d => d.Text)
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
                .OrderBy(d => d.Text)
                .ToList();
        }

        public List<LookupViewModel<short>> GetPhysicianAssessmentServices(Guid? physicianId)
        {
            return dbContext.PhysicianServices
                            .Where(p => p.PhysicianId == physicianId)
                            .Select(p => p.Service)
                            .Where(s => s.IsLocationRequired)
                            .Select(LookupDto<short>.FromServiceEntity.Expand())
                            .AsQueryable()
                            .Select(LookupViewModel<short>.FromServiceDto.Expand())
                            .ToList();
        }

        public List<SelectListItem> GetPhysicianAssessmentServiceSelectList(Guid? physicianId)
        {
            var data = GetPhysicianAssessmentServices(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .OrderBy(d => d.Text)
                .ToList();
        }

        public List<LookupViewModel<short>> GetPhysicianAddOnServices(Guid? physicianId)
        {
            return dbContext.PhysicianServices
                            .Where(p => p.PhysicianId == physicianId)
                            .Select(p => p.Service)
                            .Where(s => !s.IsLocationRequired)
                            .Select(LookupDto<short>.FromServiceEntity.Expand())
                            .AsQueryable()
                            .Select(LookupViewModel<short>.FromServiceDto.Expand())
                            .ToList();
        }

        public List<SelectListItem> GetPhysicianAddOnServiceSelectList(Guid? physicianId)
        {
            var data = GetPhysicianAddOnServices(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .OrderBy(d => d.Text)
                .ToList();
        }

        public List<Orvosi.Shared.Model.Person> GetPhysicianCaseCoordinators(Guid? physicianId)
        {
            var roles = new Guid[2] { AspNetRoles.CaseCoordinator, AspNetRoles.ExternalAdmin };
            return dbContext.Collaborators
                            .Where(c => c.UserId == physicianId)
                            .Select(c => c.CollaboratorUser)
                            .Where(c => roles.Contains(c.AspNetUserRoles.Select(r => r.RoleId).FirstOrDefault()))
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
                .OrderBy(d => d.Text)
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
                .OrderBy(d => d.Text)
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
                .OrderBy(d => d.Text)
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
            var pc = GetPhysicianCompanies(physicianId)
                .ToList();

            var query2 = pc
                .Select(c => new OwnerViewModel { Id = c.ObjectGuid, Name = c.Name });

            // retrieve any parent ids
            //var query3 = pc
            //    .Where(c => c.ParentObjectGuid.HasValue)
            //    .Select(c => new OwnerViewModel { Id = c.ParentObjectGuid, Name = c.ParentName })
            //    .Distinct();

            // concat them into 1 list
            var entities = query1.Concat(query2);//.Concat(query3);

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

            var query = data
                .Select(d => new SelectListItem
                {
                    Text = $"{d.Address.Name} - {d.Address.City}, {d.Address.Address1}",
                    Value = d.Address.Id.ToString(),
                    Group = new SelectListGroup { Name = d.Owner == null ? string.Empty : d.Owner.Name }
                });

            var noOwners = query.Where(d => string.IsNullOrEmpty(d.Group.Name))
                .OrderBy(d => d.Text);
            var owners = query.Where(d => !string.IsNullOrEmpty(d.Group.Name))
                .OrderBy(d => d.Group.Name)
                .ThenBy(d => d.Text);
            
            return owners.Concat(noOwners).ToList();
        }
        
        public class AddressViewModel
        {
            public AddressProjections.AddressResult Address { get; set; }
            public OwnerViewModel Owner { get; set; }
        }

        public const string collaboratorsKey = "collaborators";
        public List<LookupViewModel<Guid>> GetCollaborations(Guid physicianId)
        {
            var item = HttpContext.Current.Items[collaboratorsKey] as List<LookupViewModel<Guid>>;
            if (item == null)
            {
                var dto = dbContext.Collaborators
                    .Where(ww => ww.UserId == physicianId)
                    .Select(PersonDto.FromCollaboratorEntity.Expand())
                    .ToList();

                var viewModel = dto
                    .AsQueryable()
                    .Select(LookupViewModel<Guid>.FromPersonDto.Expand())
                    .ToList();

                HttpContext.Current.Items[collaboratorsKey] = item = viewModel;
            }
            return item;
        }

        public List<SelectListItem> GetCollaborationSelectList(Guid physicianId)
        {
            var data = GetCollaborations(physicianId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                    //Group = new SelectListGroup { Name = d.Role.Name }
                })
                .OrderBy(d => d.Text)
                .ToList();
        }

        public const string taskIdKey = "taskIds";
        public IEnumerable<SelectListItem> GetTaskIds(Guid userId)
        {
            var item = HttpContext.Current.Items[taskIdKey] as IEnumerable<SelectListItem>;
            if (item == null)
            {
                var dto = dbContext.ServiceRequestTasks
                    .AreActiveOrDone()
                    .AreAssignedToUser(userId)
                    .Select(srt => srt.OTask)
                    .Distinct()
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    })
                    .ToList();

                HttpContext.Current.Items[taskIdKey] = item = dto;
            }
            return item;
        }
        public const string citiesKey = "cities";
        public IEnumerable<LookupViewModel<short>> GetCities(Guid userId)
        {
            var item = HttpContext.Current.Items[citiesKey] as IEnumerable<LookupViewModel<short>>;
            if (item == null)
            {
                var dto = dbContext.ServiceRequestTasks
                    .Where(srt => srt.AssignedTo == userId)
                    .Where(srt => srt.ServiceRequest.Address != null)
                    .Select(srt => srt.ServiceRequest.Address.City_CityId)
                    .Distinct()
                    .Where(c => c != null)
                    .OrderBy(c => c.Name)
                    .Select(LookupDto<short>.FromCityEntity.Expand())
                    .ToList();

                var viewModel = dto
                    .AsQueryable()
                    .Select(LookupViewModel<short>.FromLookupDto.Expand())
                    .ToList();

                HttpContext.Current.Items[citiesKey] = item = viewModel;
            }
            return item;
        }

        public const string taskStatusIdsKey = "task-status-ids-key";
        public IEnumerable<LookupViewModel<short>> GetTaskStatuses()
        {
            var item = HttpContext.Current.Items[taskStatusIdsKey] as IEnumerable<LookupViewModel<short>>;
            if (item == null)
            {
                var dto = dbContext.TaskStatus
                    .OrderBy(c => c.Name)
                    .Select(LookupDto<short>.FromTaskStatusEntity.Expand())
                    .ToList();

                var viewModel = dto
                    .AsQueryable()
                    .Select(LookupViewModel<short>.FromLookupDto.Expand())
                    .ToList();

                HttpContext.Current.Items[taskStatusIdsKey] = item = viewModel;
            }
            return item;
        }

        public IEnumerable<SelectListItem> GetTaskStatusSelectList()
        {
            var statuses = GetTaskStatuses();
            return statuses.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        public AvailableDayDto GetPhysicianAvailableDay(Guid physicianId, DateTime day)
        {
            return dbContext.AvailableDays
                .Where(c => c.PhysicianId == physicianId && c.Day == day)
                .Select(AvailableDayDto.FromAvailableDayEntityForReschedule.Expand())
                .SingleOrDefault();
        }
        public ViewModelSelectList<AvailableSlotViewModel, short?> GetPhysicianAvailableSlotSelectList(Guid physicianId, DateTime day, short? selectedItemId)
        {
            var dto = dbContext.AvailableDays
                .Where(c => c.PhysicianId == physicianId && c.Day == day)
                .Select(AvailableDayDto.FromAvailableDayEntityForReschedule.Expand())
                .SingleOrDefault();

            var result = new ViewModelSelectList<AvailableSlotViewModel, short?>();
            if (dto == null)
            {
                return result;
            }

            result.Items = dto.AvailableSlots.AsQueryable().Select(AvailableSlotViewModel.FromAvailableSlotDto.Expand()).ToList();
            result.SelectedItemId = selectedItemId;

            return result;
        }

        public IEnumerable<LookupViewModel<Guid>> GetRequiredRoles(IEnumerable<TaskDto> tasks)
        {
            return tasks
                .Where(t => t.ResponsibleRoleId.HasValue)
                .Select(t => new LookupViewModel<Guid>
                {
                    Id = t.ResponsibleRoleId.Value,
                    Name = t.ResponsibleRoleName
                })
                .Distinct(new LookupViewModel<Guid>.LookupViewModelEquals());
        }
        public IEnumerable<SelectListItem> GetRequiredRolesSelectList(IEnumerable<TaskDto> tasks)
        {
            var roles = GetRequiredRoles(tasks);
            return roles
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });
        }

        public IEnumerable<LookupViewModel<Guid>> GetRequiredRolesFromTemplate(short? serviceRequestTemplateId)
        {
            var templateTasks = dbContext.ServiceRequestTemplateTasks
                .Where(s => s.ServiceRequestTemplateId == serviceRequestTemplateId)
                .ToList();

            return templateTasks
                .Where(t => t.ResponsibleRoleId.HasValue)
                .Select(t => new LookupViewModel<Guid>
                {
                    Id = t.AspNetRole.Id,
                    Name = t.AspNetRole.Name
                })
                .Distinct(new LookupViewModel<Guid>.LookupViewModelEquals());
        }
        public IEnumerable<SelectListItem> GetRequiredRolesFromTemplateSelectList(short? serviceRequestTemplateId)
        {
            var roles = GetRequiredRolesFromTemplate(serviceRequestTemplateId);
            return roles
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                });
        }

        public MvcHtmlString GetAvailableDays(Guid physicianId)
        {
            var availableDays = dbContext.AvailableDays.Where(c => c.PhysicianId == physicianId).ToList();

            var arr = availableDays.Select(c => string.Format("'{0}'", c.Day.ToString("yyyy-MM-dd"))).ToArray<string>();
            return MvcHtmlString.Create(string.Join(",", arr));
        }
        public const string taskTypesKey = "task-types-key";
        public IEnumerable<LookupViewModel<short>> GetTaskTypes()
        {
            var item = HttpContext.Current.Items[taskTypesKey] as IEnumerable<LookupViewModel<short>>;
            if (item == null)
            {
                var dto = dbContext.OTasks
                    .OrderBy(c => c.Name)
                    .Select(LookupDto<short>.FromTaskEntity.Expand())
                    .ToList();

                var viewModel = dto
                    .AsQueryable()
                    .Select(LookupViewModel<short>.FromLookupDto.Expand())
                    .ToList();

                HttpContext.Current.Items[taskTypesKey] = item = viewModel;
            }
            return item;
        }

        public IEnumerable<SelectListItem> GetTaskTypesSelectList()
        {
            var statuses = GetTaskTypes();
            return statuses.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        public List<SelectListItem> GetCommentTypeSelectList()
        {
            return dbContext.CommentTypes
                .OrderBy(ts => ts.Id)
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

        public List<PersonDto> GetCaseResources(int serviceRequestId)
        {
            return dbContext.ServiceRequestResources
                .Where(sr => sr.ServiceRequestId == serviceRequestId)
                .Select(PersonDto.FromServiceRequestResourceEntity.Expand())
                .ToList();
        }

        public List<SelectListItem> GetCaseResourceSelectList(int serviceRequestId)
        {
            var data = GetCaseResources(serviceRequestId);

            return data
                .Select(d => new SelectListItem
                {
                    Text = d.DisplayName,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

        public IEnumerable<LookupViewModel<byte>> GetTeleconferenceResultTypes()
        {
            return dbContext.TeleconferenceResults
                .OrderBy(ts => ts.Id)
                .Select(LookupDto<byte>.FromTeleconferenceResultEntity.Expand())
                .ToList()
                .AsQueryable()
                .Select(LookupViewModel<byte>.FromLookupDto.Expand());
        }

        public List<SelectListItem> GetTeleconferenceResultTypesSelectList()
        {
            return dbContext.TeleconferenceResults
                .OrderBy(ts => ts.Id)
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .ToList();
        }
        public IEnumerable<LookupViewModel<byte>> GetMedicolegalTypes()
        {
            return dbContext.MedicolegalTypes
                .OrderBy(ts => ts.Id)
                .Select(LookupDto<byte>.FromMedicolegalTypeEntity.Expand())
                .ToList()
                .AsQueryable()
                .Select(LookupViewModel<byte>.FromLookupDto.Expand());
        }

        public List<SelectListItem> GetMedicolegalTypeSelectList()
        {
            return dbContext.MedicolegalTypes
                .OrderBy(ts => ts.Id)
                .Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                })
                .ToList();
        }

    }
}