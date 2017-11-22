using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class CaseViewModel
    { 
        public CaseViewModel()
        {
            //Comments = new List<CommentViewModel>();
        }
        public int AvailableSlotId { get; set; }
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? AppointmentDateAndStartTime { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Notes { get; set; }
        public string BoxCaseFolderId { get; set; }
        public string BoxCaseFolderURL { get; set; }
        public decimal? ServiceCataloguePrice { get; set; }
        public bool HasAppointment { get; set; }
        public bool HasReportDeliverable { get; set; }
        public CancellationViewModel CancellationViewModel { get; set; }
        public short ServiceRequestStatusId { get; set; }
        public LookupViewModel<short> ServiceRequestStatus { get; set; }
        public bool IsOnHold { get; set; }
        public bool CanBeRescheduled { get; set; }
        public bool CanBeCancelled { get; set; }
        public bool CanBeUncancelled { get; set; }
        public bool CanBeNoShow { get; set; }
        public bool CanNoShowBeUndone { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public LookupViewModel<short> Service { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public AddressViewModel Address { get; set; }
        //public IEnumerable<ResourceViewModel> Resources { get; set; }
        public IEnumerable<TaskViewModel> Tasks { get; set; }
        public IEnumerable<MessageViewModel> Messages { get; set; }
        //public IEnumerable<CommentViewModel> Comments { get; set; }
        public LookupViewModel<Guid> CaseCoordinator { get; set; }
        public LookupViewModel<Guid> DocumentReviewer { get; set; }
        public LookupViewModel<Guid> IntakeAssistant { get; set; }
        public bool IsSubmitInvoiceTaskDone { get; set; } = false;

        public static Expression<Func<ServiceRequestDto, CaseViewModel>> FromServiceRequestDto = dto => new CaseViewModel
        {
            ServiceRequestId = dto.Id,
            ClaimantName = dto.ClaimantName,
            AppointmentDate = dto.AppointmentDate,
            StartTime = dto.StartTime,
            AppointmentDateAndStartTime = dto.AppointmentDateAndStartTime,
            DueDate = dto.DueDate,
            EffectiveDate = dto.EffectiveDate,
            Notes = dto.Notes,
            BoxCaseFolderId = dto.BoxCaseFolderId,
            BoxCaseFolderURL = dto.BoxCaseFolderURL,
            ServiceCataloguePrice = dto.ServiceCataloguePrice,
            HasAppointment = dto.HasAppointment,
            HasReportDeliverable = dto.HasReportDeliverable,
            IsOnHold = dto.ServiceRequestStatusId == ServiceRequestStatuses.OnHold,
            CanBeRescheduled = dto.CanBeRescheduled(dto.AppointmentDate),
            CanBeCancelled = dto.CanBeCancelled(dto.IsNoShow, dto.IsLateCancellation, dto.CancelledDate),
            CanBeUncancelled = dto.CanBeUncancelled(dto.IsLateCancellation, dto.CancelledDate),
            CanBeNoShow = dto.CanBeNoShow(dto.AppointmentDate),
            IsSubmitInvoiceTaskDone = dto.IsSubmitInvoiceTaskDone,
            ServiceRequestStatusId = dto.ServiceRequestStatusId,

            ServiceRequestStatus = LookupViewModel<short>.FromServiceRequestStatusDto.Invoke(dto.ServiceRequestStatus),
            CancellationViewModel = CancellationViewModel.FromServiceRequestDto.Invoke(dto),
            Service = LookupViewModel<short>.FromLookupDto.Invoke(dto.Service),
            Company = LookupViewModel<short>.FromLookupDto.Invoke(dto.Company),
            Address = AddressViewModel.FromAddressDto.Invoke(dto.Address),
            //Resources = dto.Resources.AsQueryable().Select(ResourceViewModel.FromResourceDto.Expand()),
            Physician = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.Physician),
            CaseCoordinator = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.CaseCoordinator),
            DocumentReviewer = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.DocumentReviewer),
            IntakeAssistant = LookupViewModel<Guid>.FromPersonDto.Invoke(dto.IntakeAssistant),
            Tasks = dto.Tasks.AsQueryable().Select(TaskViewModel.FromTaskDto.Expand()),
            Messages = dto.Messages.AsQueryable().Select(MessageViewModel.FromMessageDto.Expand()),
            //Comments = dto.Comments.AsQueryable().Select(CommentViewModel.FromCommentDto.Expand())
        };
    }
}