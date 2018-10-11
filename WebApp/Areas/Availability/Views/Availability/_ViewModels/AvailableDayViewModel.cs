﻿using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Address;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailableDayViewModel
    {
        public AvailableDayViewModel()
        {
            AvailableSlots = new List<AvailableSlotViewModel>();
            Resources = new List<AvailableDayResourceViewModel>();
        }
        public short Id { get; set; }
        public LookupViewModel<Guid> Physician { get; set; }
        public DateTime Day { get; set; }
        public LookupViewModel<short> Company { get; set; }
        public AddressViewModel Address { get; set; }
        public IEnumerable<AvailableSlotViewModel> AvailableSlots { get; set; }
        public IEnumerable<AvailableDayResourceViewModel> Resources { get; set; }
        public bool AreAllSlotsFilled { get; set; }
        public bool AreSomeSlotsFilled { get; set; }
        public bool AreNoSlotsFilled { get; set; }
        public bool HasSlots { get; set; }

        public static Expression<Func<AvailableDayDto, AvailableDayViewModel>> FromAvailableDayDto = e => e == null ? null : new AvailableDayViewModel
        {
            Id = e.Id,
            Physician = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(e.Physician),
            Day = e.Day,
            Company = LookupViewModel<short>.FromCompanyDto.Invoke(e.Company),
            Address = AddressViewModel.FromAddressDto.Invoke(e.Address),
            AreAllSlotsFilled = e.AreAllSlotsFilled,
            AreSomeSlotsFilled = e.AreSomeSlotsFilled,
            AreNoSlotsFilled = e.AreNoSlotsFilled,
            HasSlots = e.HasSlots,
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotViewModel.FromAvailableSlotDto.Expand()),
            Resources = e.Resources.AsQueryable().Select(AvailableDayResourceViewModel.FromAvailableDayResourceDto.Expand())
        };

        // Exclude AvailableSlots
        public static Expression<Func<AvailableDayDto, AvailableDayViewModel>> FromAvailableDayDtoForBooking = e => e == null ? null : new AvailableDayViewModel
        {
            Id = e.Id,
            Physician = LookupViewModel<Guid>.FromPersonDtoExpr.Invoke(e.Physician),
            Day = e.Day,
            Company = LookupViewModel<short>.FromCompanyDto.Invoke(e.Company),
            Address = AddressViewModel.FromAddressDto.Invoke(e.Address),
            Resources = e.Resources.AsQueryable().Select(AvailableDayResourceViewModel.FromAvailableDayResourceDto.Expand())
        };
    }
}