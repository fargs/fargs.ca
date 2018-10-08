using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailabilityViewModel : ViewModelBase
    {
        public AvailabilityViewModel(DateTime selectedDate, OrvosiDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            Calendar = CultureInfo.CurrentCulture.Calendar;
            AvailableDays = new List<AvailableDayViewModel>();
            Month = selectedDate.FirstDayOfMonth();
            MonthName = Month.ToString("MMMM yyyy");
            var nextMonth = Month.AddMonths(1);
            var availableDays = db.AvailableDays
                .Where(c => c.PhysicianId == PhysicianId)
                .Where(c => c.Day >= Month && c.Day < nextMonth)
                .Select(AvailableDayDto.FromAvailableDayEntity.Expand())
                .ToList();

            AvailableDays = availableDays.AsQueryable().Select(AvailableDayViewModel.FromAvailableDayDto);
        }

        public DateTime Month { get; set; }
        public string MonthName { get; set; }
        public IEnumerable<AvailableDayViewModel> AvailableDays { get; set; }
        public System.Globalization.Calendar Calendar { get; set; }
    }
}