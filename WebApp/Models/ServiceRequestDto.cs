using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation.Attributes;
using Orvosi.Data.Filters;
using System.Data.Entity.Core.Objects;

namespace WebApp.Models
{
    [Validator(typeof(ServiceRequestDtoValidator))]
    public partial class ServiceRequestDto //: IAppointment
    {
        public ServiceRequestDto()
        {
            //Resources = new List<ResourceDto>();
            Tasks = new List<TaskDto>();
            Messages = new List<MessageDto>();
            //Comments = new List<CommentDto>();
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

        public LookupDto<short> Service { get; set; }
        public LookupDto<short> Company { get; set; }
        public LookupDto<short> ServiceRequestStatus { get; set; }
        public AddressDto Address { get; set; }

        public PersonDto Physician { get; set; }
        public PersonDto CaseCoordinator { get; set; }
        public PersonDto DocumentReviewer { get; set; }
        public PersonDto IntakeAssistant { get; set; }

        //public IEnumerable<ResourceDto> Resources { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; }
        public IEnumerable<MessageDto> Messages { get; internal set; }
        //public IEnumerable<CommentDto> Comments { get; internal set; }

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

            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PersonDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
            CaseCoordinator = PersonDto.FromAspNetUserEntity.Invoke(sr.CaseCoordinator),
            DocumentReviewer = PersonDto.FromAspNetUserEntity.Invoke(sr.DocumentReviewer),
            IntakeAssistant = PersonDto.FromAspNetUserEntity.Invoke(sr.IntakeAssistant),

            //Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),
            Tasks = sr.ServiceRequestTasks.AsQueryable().Select(TaskDto.FromServiceRequestTaskEntity.Expand()),
            Messages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).AsQueryable().Select(MessageDto.FromServiceRequestMessageEntity.Expand()),
            //Comments = sr.ServiceRequestComments.OrderBy(srm => srm.PostedDate).AsQueryable().Select(CommentDto.FromServiceRequestCommentEntity.Expand()),
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

            ServiceRequestStatus = LookupDto<short>.FromServiceRequestStatusEntity.Invoke(sr.ServiceRequestStatu),
            Service = LookupDto<short>.FromServiceEntity.Invoke(sr.Service),
            Company = LookupDto<short>.FromCompanyEntity.Invoke(sr.Company),
            Address = AddressDto.FromAddressEntity.Invoke(sr.Address),
            Physician = PersonDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser),
            CaseCoordinator = PersonDto.FromAspNetUserEntity.Invoke(sr.CaseCoordinator),
            DocumentReviewer = PersonDto.FromAspNetUserEntity.Invoke(sr.DocumentReviewer),
            IntakeAssistant = PersonDto.FromAspNetUserEntity.Invoke(sr.IntakeAssistant)

            //Resources = sr.ServiceRequestResources.AsQueryable().Select(ResourceDto.FromServiceRequestResourceEntity.Expand()),
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromEntityForMessages = sr => new ServiceRequestDto
        {
            Id = sr.Id,
            ClaimantName = sr.ClaimantName,
            Messages = sr.ServiceRequestMessages.AsQueryable().OrderBy(m => m.PostedDate).Select(MessageDto.FromServiceRequestMessageEntity.Expand())
        };
        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForSummary = sr => new ServiceRequestDto
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

            Tasks = sr.ServiceRequestTasks.AsQueryable()
                    .Select(TaskDto.FromServiceRequestTaskEntityForSummary.Expand())
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
            Physician = PersonDto.FromAspNetUserEntity.Invoke(sr.Physician.AspNetUser)
        };

        public static Expression<Func<ServiceRequest, ServiceRequestDto>> FromServiceRequestEntityForLookAhead(short taskId)
        {
            return sr => new ServiceRequestDto
            {
                Id = sr.Id,
                AppointmentDate = sr.AppointmentDate,
                DueDate = sr.DueDate,
                ServiceRequestStatusId = sr.ServiceRequestStatusId,
                CancelledDate = sr.CancelledDate,
                IsNoShow = sr.IsNoShow,
                IsLateCancellation = sr.IsLateCancellation,

                Tasks = sr.ServiceRequestTasks.AsQueryable()
                    .Where(srt => srt.TaskId == taskId)
                    .Select(TaskDto.FromServiceRequestTaskEntityForSummary.Expand())
            };
        }

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
    }
}