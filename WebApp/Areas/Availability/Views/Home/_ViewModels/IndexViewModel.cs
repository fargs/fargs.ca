using System;
using System.Security.Principal;
using WebApp.Views.Shared;

namespace WebApp.Areas.Availability.Views.Home
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(CalendarNavigationViewModel calendarNavigation, AvailabilityViewModel availability, IIdentity identity, DateTime now) : base(identity, now)
        {
            CalendarNavigation = calendarNavigation;
            Availability = availability;
        }

        public CalendarNavigationViewModel CalendarNavigation { get; private set; }
        public AvailabilityViewModel Availability { get; }
    }
}