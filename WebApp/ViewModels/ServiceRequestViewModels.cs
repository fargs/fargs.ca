using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dropbox.Api.Files;
using Dropbox.Api.Team;
using System.Web.Mvc;
using Box.V2.Models;
using System.Data.Entity.Core.Objects;

namespace WebApp.ViewModels.ServiceRequestViewModels
{
    public class IndexViewModel
    {
        public User User { get; set; }
        public List<ServiceRequest> ServiceRequests { get; set; }
        public FilterArgs FilterArgs { get; set; }
    }

    public class DashboardViewModel
    {
        public User User { get; set; }
        public List<ServiceRequest> Today { get; set; }
        public List<ServiceRequest> Upcoming { get; set; }
    }

    public class CreateViewModel
    {
        public CreateViewModel()
        {
            this.IsAvailableDaySelected = false;
            this.IsAvailable = false;
            this.ServiceRequest = new ServiceRequest();
            this.AvailableDay = new AvailableDay();
            this.Physician = new Model.Physician();
            this.ServiceCatalogueSelected = new ServiceCatalogue();
        }
        public ServiceRequest ServiceRequest { get; set; }
        public AvailableDay AvailableDay { get; set; }
        public Model.Physician Physician { get; set; }
        public bool IsAvailableDaySelected { get; set; }
        public bool IsAvailable { get; set; }
        public bool HasCommittedToLocation { get; set; }
        public ServiceCatalogue ServiceCatalogueSelected { get; set; }
        
    }

    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            this.Resources = new List<Resource>();
        }
        public User User { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public List<ServiceRequestTask> ServiceRequestTasks{ get; set; }
        public List<ServiceRequestCostRollUp> ServiceRequestCostRollUps { get; set; }
        public Metadata DropboxFolder { get; internal set; }
        public List<MembersGetInfoItem> DropboxFolderMembers { get; internal set; }
        public IQueryable<Invoice> Invoices { get; set; }
        public BoxFolder BoxFolder { get; set; }
        public BoxCollection<BoxCollaboration> BoxFolderCollaborations { get; internal set; }
        public List<Resource> Resources { get; internal set; }
    }

    public class EditViewModel
    {
        public User User { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public List<ServiceRequestTask> ServiceRequestTasks { get; set; }
    }

    public class CreateSuccessViewModel
    {
        public CreateSuccessViewModel()
        {
            this.Errors = new ModelErrorCollection();
        }
        public ServiceRequest ServiceRequest { get; set; }
        public Invoice Invoice { get; set; }
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
        public string PhysicianId { get; set; }
        public bool? ShowAll { get; set; }
    }

    public class CancellationForm
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public DateTime? CancelledDate { get; set; }
        public string IsLate { get; set; }
        public string Notes { get; set; }

    }

    public class AvailabilityForm
    {
        [Required]
        public string PhysicianId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
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
        public AvailableDay AvailableDay { get; set; }
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
        public AvailableDay AvailableDay { get; set; }
        public bool IsAvailableDaySelected { get; set; }
        public bool IsAvailable { get; set; }
        
    }

    public class Resource
    {
        public GetServiceRequestResources_Result ResourceFromTask { get; set; }
        public BoxFolder BoxFolder { get; set; }
    }
}