using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Box.V2.Models;
using Orvosi.Data;

namespace WebApp.ViewModels.ServiceRequestViewModels
{ 

    public class IndexViewModel
    {
        public AspNetUser User { get; set; }
        public List<ServiceRequestView> ServiceRequests { get; set; }
        public FilterArgs FilterArgs { get; set; }
        public List<ServiceRequestTask> ServiceRequestTasks { get; internal set; }
    }

    public class DashboardViewModel
    {
        public AspNetUser User { get; set; }
        public List<Orvosi.Data.ServiceRequest> Today { get; set; }
        public List<Orvosi.Data.ServiceRequest> Upcoming { get; set; }
    }

    public class CreateViewModel
    {
        public Orvosi.Data.ServiceRequest ServiceRequest { get; set; }
        public Orvosi.Data.AvailableDay SelectedAvailableDay { get; set; }
        public Orvosi.Data.Physician SelectedPhysician { get; set; }
        public IEnumerable<SelectListItem> ServiceSelectList { get; set; }
        public IEnumerable<SelectListItem> AvailableSlotSelectList { get; set; }
        public IEnumerable<SelectListItem> CompanySelectList { get; set; }
        public IEnumerable<SelectListItem> AddressSelectList { get; set; }
        public IEnumerable<SelectListItem> StaffSelectList { get; set; }
        public IEnumerable<SelectListItem> PhysicianSelectList { get; set; }
        public IEnumerable<SelectListItem> ServiceRequestTemplateSelectList { get; internal set; }
    }

    public class DetailsViewModel
    {
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
        public Orvosi.Data.ServiceRequest ServiceRequest { get; set; }
        public WebApp.Models.ServiceRequestModels.ServiceRequest ServiceRequestMapped { get; set; }
        public IEnumerable<WebApp.Models.ServiceRequestModels.ServiceRequestTask> TaskList { get; internal set; }
    }

    public class BoxManagerViewModel
    {
        public BoxManagerViewModel()
        {
            this.Resources = new List<BoxResource>();
        }
        public int ServiceRequestId { get; set; }
        public BoxFolder BoxFolder { get; set; }
        public BoxCollection<BoxCollaboration> BoxFolderCollaborations { get; internal set; }
        public List<BoxResource> Resources { get; internal set; }
    }

    public class EditViewModel
    {
        public int ServiceRequestId { get; set; }
        public string ClaimantName { get; set; }
        public DateTime? DueDate { get; set; }
        public string CompanyReferenceId { get; set; }
        public string AdditionalNotes { get; set; }
        public Guid? RequestedBy { get; set; }
        public DateTime? RequestedDate { get; set; }
    }

    public class CreateSuccessViewModel
    {
        public CreateSuccessViewModel()
        {
            this.Errors = new ModelErrorCollection();
        }
        public Orvosi.Data.ServiceRequest ServiceRequest { get; set; }
        public Orvosi.Data.Invoice Invoice { get; set; }
        public Dropbox.Api.Files.Metadata Folder { get; set; }
        public List<Dropbox.Api.Team.MembersGetInfoItem> Members { get; set; }
        public ModelErrorCollection Errors { get; set; }
    }

    public class FilterArgs
    {
        public string Sort { get; set; }
        public byte? DateRange { get; set; }
        public byte? StatusId { get; set; }
        public string NextTask { get; set; }
        public string Ids { get; set; }
        public string ClaimantName { get; set; }
        public Guid? PhysicianId { get; set; }
        public bool? ShowAll { get; set; }
    }

    public class CancellationForm
    {
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        public DateTime CancelledDate { get; set; }
        public string IsLate { get; set; }
        public string Notes { get; set; }

    }

    public class AvailabilityForm
    {
        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
    }

    public class ResourceAssignmentViewModel
    {
        public int ServiceRequestId { get; set; }
        public Guid? CaseCoordinatorId { get; set; }
        public Guid? DocumentReviewerId { get; set; }
        public Guid? IntakeAssistantId { get; set; }
        public IEnumerable<SelectListItem> UserSelectList { get; set; }
    }

    public class CompanyForm
    {
        [Required]
        public string PhysicianId { get; set; }
        public string PhysicianName { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public short? CompanyId { get; set; }
        public Orvosi.Data.AvailableDay AvailableDay { get; set; }
    }

    public class LocationForm
    {
        [Required]
        public string PhysicianId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public short? CompanyId { get; set; }
        [Required]
        public short? LocationId { get; set; }
        public string PhysicianName { get; set; }
        public string LocationName { get; set; }
        public string CompanyName { get; set; }
        public bool? IsParentCompany { get; set; }
        public bool IsPrebook { get; internal set; }
        public Orvosi.Data.AvailableDay AvailableDay { get; set; }
        public bool IsAvailableDaySelected { get; set; }
        public bool IsAvailable { get; set; }
        
    }

    public class BoxResource
    {
        public AspNetUser Resource { get; set; }
        public BoxFolder BoxFolder { get; set; }
    }
    public class CaseSearchListItem
    {
        public int Id { get; set; }
        public string Claimant { get; set; }
        public string Url { get; set; }
    }

}