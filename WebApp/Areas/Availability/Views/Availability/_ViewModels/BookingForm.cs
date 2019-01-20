using FluentDateTime;
using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Availability.Views.Shared;
using WebApp.Library.Extensions;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class BookingForm : ViewModelBase
    {
        private ImeHubDbContext db;
        public BookingForm()
        {

        }
        public BookingForm(Guid availableSlotId, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            this.db = db;

            var slot = db.AvailableSlots
                .AsNoTracking()
                .AsExpandable()
                .Where(a => a.Id == availableSlotId)
                .Select(AvailableSlotModel.FromAvailableSlotEntityForBooking)
                .Single();

            AvailableSlotId = availableSlotId;
            AvailableSlot = AvailableSlotViewModel.FromAvailableSlotModelForBooking.Invoke(slot);

            CompanyId = slot.AvailableDay.Company == null ? (Guid?)null : slot.AvailableDay.Company.Id;
            AddressId = slot.AvailableDay.Address == null ? (Guid?)null : slot.AvailableDay.Address.Id;

            AppointmentDate = slot.AvailableDay.Day;
            DueDate = AppointmentDate.AddDays(3);

            ViewData = new ViewDataModel(availableSlotId, db, PhysicianId.Value, CompanyId, now);
        }


        public Guid AvailableSlotId { get; set; }
        public AvailableSlotViewModel AvailableSlot { get; set; }

        [Required]
        public Guid? CompanyId { get; set; }
        
        [Required]
        public Guid? AddressId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public Guid ServiceId { get; set; }
        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now;
        public string CompanyReferenceId { get; set; }
        [Required]
        public string ClaimantName { get; set; }
        [Required]
        public short ServiceRequestTemplateId { get; set; }
        public string ReferralSource { get; set; }
        public Guid WorkflowId { get; set; }

        public ViewDataModel ViewData { get; set; }

        public class ViewDataModel
        {
            private ImeHubDbContext db;
            private Guid physicianId;
            public ViewDataModel(Guid availableSlotId, ImeHubDbContext db, Guid physicianId, Guid? companyId, DateTime now)
            {
                this.db = db;
                this.physicianId = physicianId;
                Companies = GetPhysicianCompanySelectList();
                Addresses = GetPhysicianAddressSelectList();
                Services = GetServices();
                Workflows = GetWorkflows();
                
            }
            public IEnumerable<SelectListItem> Services { get; set; }
            public IEnumerable<SelectListItem> Companies { get; set; }
            public IEnumerable<SelectListItem> Addresses { get; set; }
            public IEnumerable<SelectListItem> Workflows { get; set; }

            private List<SelectListItem> GetPhysicianCompanySelectList()
            {
                var companies = db.Companies
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(CompanyModel.FromCompany)
                    .ToList();

                return companies
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }

            private List<SelectListItem> GetServices()
            {
                var services = db.Services
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.IsTravelRequired)
                    .Distinct()
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();

                return services;
                    
            }
            private IEnumerable<SelectListItem> GetPhysicianAddressSelectList()
            {

                var physicianAddresses = db.Addresses
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(a => a.PhysicianId == physicianId)
                    .Select(AddressModel.FromAddress)
                    .ToList()
                    .Select(AddressViewModel.FromAddressModel);

                var companyIds = db.Companies
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(c => c.PhysicianId == physicianId)
                    .Select(c => c.Id)
                    .ToArray();

                var companyAddresses = db.Addresses
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(a => a.CompanyId.HasValue)
                    .Where(p => companyIds.Contains(p.CompanyId.Value))
                    .Select(AddressModel.FromAddress)
                    .ToList()
                    .Select(AddressViewModel.FromAddressModel);

                var addresses = physicianAddresses.Concat(companyAddresses);

                var list = addresses
                    .Select(d => new SelectListItem
                    {
                        Text = $"{d.Name} - {d.City}, {d.Address1}",
                        Value = d.Id.ToString(),
                        Group = new SelectListGroup { Name = d.Owner }
                    })
                    .OrderBy(d => d.Group.Name)
                    .ThenBy(d => d.Text);

                return list;
            }
            private List<SelectListItem> GetWorkflows()
            {
                var workflows = db.Workflows
                    .AsNoTracking()
                    .AsExpandable()
                    .Where(p => p.PhysicianId == physicianId)
                    .Select(w => new
                    {
                        w.Id,
                        w.Name
                    })
                    .ToList();

                return workflows
                    .Select(d => new SelectListItem
                    {
                        Text = d.Name,
                        Value = d.Id.ToString()
                    })
                    .OrderBy(d => d.Text)
                    .ToList();
            }
        }
        
    }
}