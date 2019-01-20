using FluentDateTime;
using LinqKit;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using ImeHub.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class AvailabilityViewModel : ViewModelBase
    {
        public AvailabilityViewModel(DateTime selectedDate, ImeHubDbContext db, IIdentity identity, DateTime now) : base(identity, now)
        {
            Calendar = CultureInfo.CurrentCulture.Calendar;
            Month = selectedDate.FirstDayOfMonth();
            MonthName = Month.ToString("MMMM yyyy");
            var nextMonth = Month.AddMonths(1);
            var availableDays = db.AvailableDays
                .AsNoTracking()
                .AsExpandable()
                .Where(c => c.PhysicianId == PhysicianId)
                .Where(c => c.Day >= Month && c.Day < nextMonth)
                .Select(AvailableDayModel.FromAvailableDayEntity.Expand())
                .ToList();

                AvailableDays = availableDays.Select(AvailableDayViewModel.FromAvailableDayDto);
        }

        public DateTime Month { get; set; }
        public string MonthName { get; set; }
        public IEnumerable<AvailableDayViewModel> AvailableDays { get; set; }
        public System.Globalization.Calendar Calendar { get; set; }
    }
}