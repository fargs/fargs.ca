using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ImeHub.Models
{
    public class AvailableDayModel
    {
        public Guid Id { get; set; }
        public PhysicianModel Physician { get; set; }
        public DateTime Day { get; set; }
        public LookupModel<Guid> Company { get; set; }
        public AddressModel Address { get; set; }
        public IEnumerable<AvailableSlotModel> AvailableSlots { get; set; }
        public IEnumerable<AvailableDayResourceModel> Resources { get; set; }

        public bool AreAllSlotsFilled
        {
            get
            {
                return AvailableSlots.Any() && AvailableSlots.All(a => a.ServiceRequestIds.Any()) ? true : false;
            }
        }
        public bool AreSomeSlotsFilled
        {
            get
            {
                return AvailableSlots.Any(a => a.ServiceRequestIds.Any()) ? true : false;
            }
        }
        public bool AreNoSlotsFilled
        {
            get
            {
                return AvailableSlots.Any() && !AreSomeSlotsFilled ? true : false;
            }
        }
        public bool HasSlots
        {
            get
            {
                return AvailableSlots.Any();
            }
        }

        public static Expression<Func<AvailableDay, AvailableDayModel>> FromAvailableDayEntity = e => e == null ? null : new AvailableDayModel
        {
            Id = e.Id,
            Physician = PhysicianModel.FromPhysician.Invoke(e.Physician),
            Day = e.Day,
            Company = LookupModel<Guid>.FromCompany.Invoke(e.Company),
            Address = AddressModel.FromAddress.Invoke(e.Address),
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotModel.FromAvailableSlot),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceModel.FromAvailableDayResource)
        };

        public static Expression<Func<AvailableDay, AvailableDayModel>> FromAvailableDayEntityForReschedule = e => e == null ? null : new AvailableDayModel
        {
            Id = e.Id,
            Physician = PhysicianModel.FromPhysician.Invoke(e.Physician),
            Day = e.Day,
            Company = LookupModel<Guid>.FromCompany.Invoke(e.Company),
            Address = AddressModel.FromAddress.Invoke(e.Address),
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotModel.FromAvailableSlotForReschedule.Expand()),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceModel.FromAvailableDayResource.Expand())
        };

        public static Expression<Func<AvailableDay, AvailableDayModel>> FromAvailableDayEntityForDayView = e => e == null ? null : new AvailableDayModel
        {
            Id = e.Id,
            Physician = PhysicianModel.FromPhysician.Invoke(e.Physician),
            Day = e.Day,
            Company = LookupModel<Guid>.FromCompany.Invoke(e.Company),
            Address = AddressModel.FromAddress.Invoke(e.Address),
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotModel.FromAvailableSlotEntityForDayView.Expand()),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceModel.FromAvailableDayResource.Expand())
        };

        // exclude Available Slots
        public static Expression<Func<AvailableDay, AvailableDayModel>> FromAvailableDayEntityForBooking = e => e == null ? null : new AvailableDayModel
        {
            Id = e.Id,
            Physician = PhysicianModel.FromPhysician.Invoke(e.Physician),
            Day = e.Day,
            Company = LookupModel<Guid>.FromCompany.Invoke(e.Company),
            Address = AddressModel.FromAddress.Invoke(e.Address),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceModel.FromAvailableDayResource.Expand())
        };
    }
}