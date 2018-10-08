using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation.Attributes;
using Orvosi.Data.Filters;

namespace WebApp.Models
{
    [Validator(typeof(ServiceRequestDtoValidator))]
    public partial class ServiceRequestDto //: IAppointment
    {
        public ServiceRequestDto()
        {
            Resources = new List<ResourceDto>();
            Tasks = new List<TaskDto>();
            Messages = new List<MessageDto>();
            Comments = new List<CommentDto>();
            InvoiceDetails = new List<InvoiceDetailDto>();
            Teleconferences = new List<TeleconferenceDto>();
        }

        public int Id { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public bool IsNoShow { get; set; }
        public bool IsLateCancellation { get; set; }
        public DateTime? CancelledDate { get; set; }
        public short ServiceRequestStatusId { get; internal set; }
        public string BoxCaseFolderId { get; set; }
        public decimal? ServiceCataloguePrice { get; set; }
        public string Notes { get; set; }
        public bool HasErrors { get; set; }
        public bool HasWarnings { get; set; }
        public string SourceCompany { get; set; }
        public byte? MedicolegalTypeId { get; set; }

        // Computed properties
        public bool IsCancelled => this.CancelledDate.HasValue;


        // Functions
        public bool IsLateCancellationPolicyViolated(int? lateCancellationPolicy, DateTime cancelledDate)
        {
            //TODO: This potentially should move to the ServiceRequestTask on event tasks to allow for multiple events per case

            if (!lateCancellationPolicy.HasValue)
            {
                return false;
            }

            // does not have an appointment
            if (!HasAppointment)
            {
                return false;
            }

            if (cancelledDate >= AppointmentDate)
            {
                return true;
            }

            TimeSpan diff = AppointmentDate.Value - cancelledDate;
            double hours = diff.TotalHours;

            if (diff.TotalHours <= lateCancellationPolicy)
            {
                return true;
            }

            return false;
        }

        // Foreign Keys
        public Guid? PhysicianId { get; set; }
        public int? ServiceId { get; set; }
        public int? CompanyId { get; set; } // used to get the service catalogue rates
        public Guid? CompanyGuid { get; set; } // used to get the service catalogue in the accounting controller.
        public short? ServiceRequestTemplateId { get; set; }

        public LookupDto<byte> MedicolegalType { get; set; }
        public LookupDto<short> Service { get; set; }
        public LookupDto<short> Company { get; set; }
        public LookupDto<short> ServiceRequestStatus { get; set; }
        public LookupDto<short> NextTaskStatusForUser { get; set; }
        public AddressDto Address { get; set; }

        public PhysicianDto Physician { get; set; }
        public PersonDto CaseCoordinator { get; set; }
        public PersonDto DocumentReviewer { get; set; }
        public PersonDto IntakeAssistant { get; set; }

        public IEnumerable<ResourceDto> Resources { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
        public IEnumerable<CommentDto> Comments { get; internal set; }
        public IEnumerable<TeleconferenceDto> Teleconferences { get; internal set; }
        public IEnumerable<InvoiceDetailDto> InvoiceDetails { get; set; }

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntity = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,
            HasErrors = sr.HasErrors,
            HasWarnings = sr.HasWarnings,
            BoxCaseFolderId = sr.BoxCaseFolderId,
            ServiceCataloguePrice = sr.ServiceCataloguePrice,
            Notes = sr.Notes,
            ServiceRequestTemplateId = sr.ServiceRequestTemplateId,
            SourceCompany = sr.SourceCompany,
            MedicolegalTypeId = sr.MedicolegalTypeId,

            MedicolegalType = LookupDto<byte>.FromMedicolegalTypeEntity.Invoke(sr.MedicolegalType),
            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),

            Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),
            Tasks = sr.ServiceRequestTasks.AsQueryable().Select(TaskDto.FromServiceRequestTaskEntity.Expand()),
            Messages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).AsQueryable().Select(MessageDto.FromServiceRequestMessageEntity.Expand()),
            Comments = sr.ServiceRequestComments.OrderBy(srm => srm.PostedDate).AsQueryable().Select(CommentDto.FromServiceRequestCommentEntity.Expand()),
            Teleconferences = sr.Teleconferences.OrderBy(t => t.AppointmentDate).ThenBy(t => t.StartTime).AsQueryable().Select(TeleconferenceDto.FromEntity.Expand()),
            InvoiceDetails = sr.InvoiceDetails.AsQueryable().Select(InvoiceDetailDto.FromInvoiceDetailEntity.Expand())
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityV2(Guid userId)
        {
            return sr => new ServiceRequestDto {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                AppointmentDate = sr.AppointmentDate,
                StartTime = sr.StartTime,
                DueDate = sr.DueDate,
                CancelledDate = sr.CancelledDate,
                IsNoShow = sr.IsNoShow,
                IsLateCancellation = sr.IsLateCancellation,
                ServiceRequestStatusId = sr.ServiceRequestStatusId,
                HasErrors = sr.HasErrors,
                HasWarnings = sr.HasWarnings,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                ServiceCataloguePrice = sr.ServiceCataloguePrice,
                Notes = sr.Notes,
                ServiceRequestTemplateId = sr.ServiceRequestTemplateId,
                SourceCompany = sr.SourceCompany,
                MedicolegalTypeId = sr.MedicolegalTypeId,

                MedicolegalType = LookupDto<byte>.FromMedicolegalTypeEntity.Invoke(sr.MedicolegalType),
                ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
                Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
                Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
                Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
                Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),

                Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),
                Tasks = sr.ServiceRequestTasks.AsQueryable().Select(TaskDto.FromServiceRequestTaskEntity.Expand()),
                Messages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).AsQueryable().Select(MessageDto.FromServiceRequestMessageEntity.Expand()),
                Teleconferences = sr.Teleconferences.OrderBy(t => t.AppointmentDate).ThenBy(t => t.StartTime).AsQueryable().Select(TeleconferenceDto.FromEntity.Expand()),
                // The security is applied to the comments at the query/projection line below (access list, posted by, and physician)
                // Refactored this Where clause out to Orvosi.Data.Filters.ServiceRequestCommentFilters.CanAccess but it does not translate into SQL
                Comments = sr.ServiceRequestComments.Where(c => c.ServiceRequestCommentAccesses.Select(a => a.AspNetUser.Id).Contains(userId) || c.AspNetUser.Id == userId || c.ServiceRequest.PhysicianId == userId).OrderBy(srm => srm.PostedDate).AsQueryable().Select(CommentDto.FromServiceRequestCommentEntity.Expand()),
                InvoiceDetails = sr.InvoiceDetails.AsQueryable().Select(InvoiceDetailDto.FromInvoiceDetailEntity.Expand())
            };
        }

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForCaseNotification = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,

            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser)
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForCase = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,
            HasErrors = sr.HasErrors,
            HasWarnings = sr.HasWarnings,
            BoxCaseFolderId = sr.BoxCaseFolderId,
            ServiceCataloguePrice = sr.ServiceCataloguePrice,
            Notes = sr.Notes,
            SourceCompany = sr.SourceCompany,
            MedicolegalTypeId = sr.MedicolegalTypeId,

            MedicolegalType = LookupDto<byte>.FromMedicolegalTypeEntity.Invoke(sr.MedicolegalType),
            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
            Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand())
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForTasks = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,
            HasErrors = sr.HasErrors,
            HasWarnings = sr.HasWarnings,
            BoxCaseFolderId = sr.BoxCaseFolderId,
            ServiceCataloguePrice = sr.ServiceCataloguePrice,
            Notes = sr.Notes,
            SourceCompany = sr.SourceCompany,
            MedicolegalTypeId = sr.MedicolegalTypeId,

            MedicolegalType = LookupDto<byte>.FromMedicolegalTypeEntity.Invoke(sr.MedicolegalType),
            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PhysicianDto.FromPhysicianEntity.Invoke(sr.Physician.AspNetUser),
            Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand())
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForDaySheet(Guid userId)
        {
            return sr => new ServiceRequestDto
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                AppointmentDate = sr.AppointmentDate,
                StartTime = sr.StartTime,
                DueDate = sr.DueDate,
                CancelledDate = sr.CancelledDate,
                IsNoShow = sr.IsNoShow,
                IsLateCancellation = sr.IsLateCancellation,
                HasErrors = sr.HasErrors,
                HasWarnings = sr.HasWarnings,
                BoxCaseFolderId = sr.BoxCaseFolderId,
                ServiceCataloguePrice = sr.ServiceCataloguePrice,
                Notes = sr.Notes,
                SourceCompany = sr.SourceCompany,

                MedicolegalTypeId = sr.MedicolegalTypeId,
                MedicolegalType = LookupDto<byte>.FromMedicolegalTypeEntity.Invoke(sr.MedicolegalType),
                ServiceRequestStatusId = sr.ServiceRequestStatusId,
                ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
                ServiceId = sr.ServiceId,
                Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
                CompanyId = sr.CompanyId,
                Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
                Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
                PhysicianId = sr.PhysicianId,
                Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),

                Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),
                Comments = sr.ServiceRequestComments.Where(c => c.ServiceRequestCommentAccesses.Select(a => a.AspNetUser.Id).Contains(userId) || c.AspNetUser.Id == userId || c.ServiceRequest.PhysicianId == userId).OrderBy(srm => srm.PostedDate).AsQueryable().Select(CommentDto.FromServiceRequestCommentEntity.Expand()),
                Tasks = sr.ServiceRequestTasks.AsQueryable().Select(TaskDto.FromServiceRequestTaskEntity.Expand()),
                Teleconferences = sr.Teleconferences.OrderBy(t => t.AppointmentDate).ThenBy(t => t.StartTime).AsQueryable().Select(TeleconferenceDto.FromEntity.Expand()),
                InvoiceDetails = sr.InvoiceDetails.AsQueryable().Select(InvoiceDetailDto.FromInvoiceDetailEntity.Expand())
            };
        }

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromEntityForAvailability = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            PhysicianId = sr.PhysicianId,
            CompanyId = sr.CompanyId,
            ServiceId = sr.ServiceId,
            AppointmentDate = sr.AppointmentDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromEntityForCancellationForm = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            PhysicianId = sr.PhysicianId,
            CompanyId = sr.CompanyId,
            ServiceId = sr.ServiceId,
            AppointmentDate = sr.AppointmentDate
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromEntityForMessages = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            Messages = sr.ServiceRequestMessages.AsQueryable().OrderBy(m => m.PostedDate).Select(MessageDto.FromServiceRequestMessageEntity.Expand())
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForCaseLinks(Guid userId)
        {
            return sr => new ServiceRequestDto
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                AppointmentDate = sr.AppointmentDate,
                StartTime = sr.StartTime,
                DueDate = sr.DueDate,
                CancelledDate = sr.CancelledDate,
                IsNoShow = sr.IsNoShow,
                IsLateCancellation = sr.IsLateCancellation,
                ServiceRequestStatusId = sr.ServiceRequestStatusId,
                HasErrors = sr.HasErrors,
                HasWarnings = sr.HasWarnings,
                Notes = sr.Notes,

                ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
                Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
                Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
                Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
                Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
                Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),
                CaseCoordinator = PersonDto.FromAspNetUserEntity.Invoke(sr.CaseCoordinator),
                DocumentReviewer = PersonDto.FromAspNetUserEntity.Invoke(sr.DocumentReviewer),
                IntakeAssistant = PersonDto.FromAspNetUserEntity.Invoke(sr.IntakeAssistant),

                NextTaskStatusForUser = sr.ServiceRequestTasks
                    .AsQueryable()
                    .Where(srt => srt.AssignedTo == userId)
                    .GroupBy(srt => srt.ServiceRequestId)
                    .Select(srt => srt.OrderBy(grp => grp.TaskStatu.ServiceRequestPrecedence).FirstOrDefault().TaskStatu)
                    .Select(ts => new LookupDto<short>
                    {
                        Id = ts.Id,
                        Name = ts.Name,
                        Code = "",
                        ColorCode = ts.ColorCode
                    })
                    .FirstOrDefault()
            };
        }

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForSchedule(Guid userId)
        {
            return sr => new ServiceRequestDto
            {
                Id = sr.Id,
                ClaimantName = sr.ClaimantName,
                AppointmentDate = sr.AppointmentDate,
                StartTime = sr.StartTime,
                DueDate = sr.DueDate,
                CancelledDate = sr.CancelledDate,
                IsNoShow = sr.IsNoShow,
                IsLateCancellation = sr.IsLateCancellation,
                ServiceRequestStatusId = sr.ServiceRequestStatusId,
                
                ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
                Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
                Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
                Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
                Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
                Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),

                NextTaskStatusForUser = LookupDto<short>.ForNextTaskStatusForUser.Invoke(sr.ServiceRequestTasks.AsQueryable(), userId)
            };
        }

        public static Expression<Func<ServiceRequest, Guid, ServiceRequestDto>> FromServiceRequestEntityForSummary = (sr, userId) => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,
            HasErrors = sr.HasErrors,
            HasWarnings = sr.HasWarnings,

            Tasks = sr.ServiceRequestTasks
                    .AsQueryable()
                    .AreAssignedToUser(userId)
                    .Select(srt => new TaskDto
                    {
                        Id = srt.Id,
                        TaskStatusId = srt.TaskStatusId
                    })
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntitySummary = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,
            HasErrors = sr.HasErrors,
            HasWarnings = sr.HasWarnings,

            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser)
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForInvoice = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            AppointmentDate = sr.AppointmentDate,
            StartTime = sr.StartTime,
            DueDate = sr.DueDate,
            CancelledDate = sr.CancelledDate,
            IsNoShow = sr.IsNoShow,
            IsLateCancellation = sr.IsLateCancellation,
            ServiceRequestStatusId = sr.ServiceRequestStatusId,
            Notes = sr.Notes,
            CompanyGuid = sr.Company.ObjectGuid,

            Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),

            Tasks = sr.ServiceRequestTasks.AsQueryable()
                    .Where(srt => srt.TaskId == Orvosi.Shared.Enums.Tasks.SubmitInvoice)
                    .Select(TaskDto.FromServiceRequestTaskEntityForInvoiceDetail.Expand())
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForTaskStatusSummary(Guid userId, short[] taskIds)
        {
            return sr => new ServiceRequestDto
            {
                Id = sr.Id,
                AppointmentDate = sr.AppointmentDate,

                Tasks = sr.ServiceRequestTasks.AsQueryable()
                    .AreAssignedToUser(userId)
                    .WithTaskIds(taskIds)
                    .AsQueryable()
                    .Select(TaskDto.FromServiceRequestTaskEntityForSummary.Expand())
            };
        }

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForInvoiceDetail = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            Tasks = sr.ServiceRequestTasks.AsQueryable()
                .Where(srt => srt.TaskId == Orvosi.Shared.Enums.Tasks.SubmitInvoice)
                .Select(TaskDto.FromServiceRequestTaskEntityForInvoiceDetail.Expand())
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForTeleconferenceNotification = sr => new ServiceRequestDto
        {
            Id =  sr.Id,
            ClaimantName = sr.ClaimantName,
            PhysicianId = sr.PhysicianId,
            Physician = PhysicianDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser)
        };
    }
}