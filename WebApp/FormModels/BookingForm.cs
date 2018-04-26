using LinqKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.FormModels
{
    public class BookingForm
    {
        public int AvailableSlotId { get; set; }
        public AvailableSlotViewModel AvailableSlotViewModel { get; set; }

        private short? _CompanyId;
        [Required]
        public short? CompanyId {
            get
            {
                return _CompanyId ?? (AvailableSlotViewModel?.AvailableDay?.Company == null ? (short?)null : AvailableSlotViewModel.AvailableDay.Company.Id);
            }
            set
            {
                _CompanyId = value;
            }
        }

        private int? _AddressId;
        [Required]
        public int? AddressId
        {
            get
            {
                return _AddressId ?? (AvailableSlotViewModel?.AvailableDay?.Address == null ? (int?)null : AvailableSlotViewModel.AvailableDay.Address.Id);
            }
            set
            {
                _AddressId = value;
            }
        }

        //private IEnumerable<AvailableDayResourceViewModel> _Resources;
        //public IEnumerable<AvailableDayResourceViewModel> Resources
        //{
        //    get
        //    {
        //        return _Resources ?? AvailableSlotViewModel.AvailableDay.Resources;
        //    }
        //    set
        //    {
        //        _Resources = value;
        //    }
        //}
        [Required]
        public DateTime AppointmentDate { get; set; }
        [Required]
        public short ServiceId { get; set; }
        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now;
        public string CompanyReferenceId { get; set; }
        [Required]
        public string ClaimantName { get; set; }
        [Required]
        public short ServiceRequestTemplateId { get; set; }
        public string SourceCompany { get; set; }


        public static Expression<Func<AvailableSlotDto, BookingForm>> FromAvailableSlotDto = r => r == null ? null : new BookingForm
        {
            AvailableSlotId = r.Id,
            AvailableSlotViewModel = AvailableSlotViewModel.FromAvailableSlotDtoForBooking.Invoke(r),
        };
    }
}