using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class AvailableDayDto
    {
        public short Id { get; set; }
        public PersonDto Physician { get; set; }
        public DateTime Day { get; set; }
        public LookupDto<short> Company { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<AvailableSlotDto> AvailableSlots { get; set; }
        public IEnumerable<AvailableDayResourceDto> Resources { get; set; }

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

        public static Expression<Func<AvailableDay, AvailableDayDto>> FromAvailableDayEntity = e => e == null ? null : new AvailableDayDto
        {
            Id = e.Id,
            Physician = PersonDto.FromAspNetUserEntityWithRole.Invoke(e.Physician.AspNetUser),
            Day = e.Day,
            Company = LookupDto<short>.FromCompanyEntity.Invoke(e.Company),
            Address = AddressDto.FromAddressEntity.Invoke(e.Address),
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotDto.FromAvailableSlotEntity.Expand()),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceDto.FromAvailableDayResourceEntity.Expand())
        };

        public static Expression<Func<AvailableDay, AvailableDayDto>> FromAvailableDayEntityForReschedule = e => e == null ? null : new AvailableDayDto
        {
            Id = e.Id,
            Physician = PersonDto.FromAspNetUserEntityWithRole.Invoke(e.Physician.AspNetUser),
            Day = e.Day,
            Company = LookupDto<short>.FromCompanyEntity.Invoke(e.Company),
            Address = AddressDto.FromAddressEntity.Invoke(e.Address),
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotDto.FromAvailableSlotEntityForReschedule.Expand()),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceDto.FromAvailableDayResourceEntity.Expand())
        };

        public static Expression<Func<AvailableDay, AvailableDayDto>> FromAvailableDayEntityForDayView = e => e == null ? null : new AvailableDayDto
        {
            Id = e.Id,
            Physician = PersonDto.FromAspNetUserEntityWithRole.Invoke(e.Physician.AspNetUser),
            Day = e.Day,
            Company = LookupDto<short>.FromCompanyEntity.Invoke(e.Company),
            Address = AddressDto.FromAddressEntity.Invoke(e.Address),
            AvailableSlots = e.AvailableSlots.AsQueryable().Select(AvailableSlotDto.FromAvailableSlotEntityForDayView.Expand()),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceDto.FromAvailableDayResourceEntity.Expand())
        };

        // exclude Available Slots
        public static Expression<Func<AvailableDay, AvailableDayDto>> FromAvailableDayEntityForBooking = e => e == null ? null : new AvailableDayDto
        {
            Id = e.Id,
            Physician = PersonDto.FromAspNetUserEntityWithRole.Invoke(e.Physician.AspNetUser),
            Day = e.Day,
            Company = LookupDto<short>.FromCompanyEntity.Invoke(e.Company),
            Address = AddressDto.FromAddressEntity.Invoke(e.Address),
            Resources = e.AvailableDayResources.AsQueryable().Select(AvailableDayResourceDto.FromAvailableDayResourceEntity.Expand())
        };
    }
}