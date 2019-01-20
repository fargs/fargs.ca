using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ImeHub.Models;
using WebApp.Areas.Availability.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayViewModel
    {
        public AvailableDayViewModel()
        {
            AvailableSlots = new List<AvailableSlotViewModel>();
            Resources = new List<AvailableDayResourceViewModel>();
        }
        public Guid Id { get; set; }
        public PhysicianViewModel Physician { get; set; }
        public DateTime Day { get; set; }
        public CompanyViewModel Company { get; set; }
        public AddressViewModel Address { get; set; }
        public IEnumerable<AvailableSlotViewModel> AvailableSlots { get; set; }
        public IEnumerable<AvailableDayResourceViewModel> Resources { get; set; }
        public bool AreAllSlotsFilled { get; set; }
        public bool AreSomeSlotsFilled { get; set; }
        public bool AreNoSlotsFilled { get; set; }
        public bool HasSlots { get; set; }

        public static Func<AvailableDayModel, AvailableDayViewModel> FromAvailableDayDto = e => e == null ? null : new AvailableDayViewModel
        {
            Id = e.Id,
            Physician = new PhysicianViewModel(e.Physician),
            Day = e.Day,
            Company = new CompanyViewModel(e.Company),
            Address = AddressViewModel.FromAddressModel(e.Address),
            AreAllSlotsFilled = e.AreAllSlotsFilled,
            AreSomeSlotsFilled = e.AreSomeSlotsFilled,
            AreNoSlotsFilled = e.AreNoSlotsFilled,
            HasSlots = e.HasSlots,
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotViewModel.FromAvailableSlotModel),
            Resources = e.Resources.AsQueryable().Select(AvailableDayResourceViewModel.FromAvailableDayResourceModel)
        };

        // Exclude AvailableSlots
        public static Expression<Func<AvailableDayModel, AvailableDayViewModel>> FromAvailableDayModelForBooking = e => e == null ? null : new AvailableDayViewModel
        {
            Id = e.Id,
            Physician = new PhysicianViewModel(e.Physician),
            Day = e.Day,
            Company = new CompanyViewModel(e.Company),
            Address = AddressViewModel.FromAddressModel(e.Address),
            Resources = e.Resources.AsQueryable().Select(AvailableDayResourceViewModel.FromAvailableDayResourceModel)
        };
    }
}