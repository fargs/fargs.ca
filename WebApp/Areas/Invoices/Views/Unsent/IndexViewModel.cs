using System;
using System.Security.Principal;
using WebApp.Views.Shared;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class IndexViewModel : ViewModelBase
    {
        public IndexViewModel(CalendarNavigationViewModel calendarNavigation, UnsentViewModel unsent, IIdentity identity, DateTime now) : base(identity, now)
        {
            CalendarNavigation = calendarNavigation;
            Unsent = unsent;
        }

        public CalendarNavigationViewModel CalendarNavigation { get; private set; }
        public UnsentViewModel Unsent { get; private set; }
    }
}