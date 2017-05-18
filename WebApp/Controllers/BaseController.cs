using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library;
using WebApp.Library.Extensions;
using WebApp.ViewModels;
using FluentDateTime;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected WorkService service;
        protected AccountingService accountingService;
        protected OrvosiDbContext db;
        protected IIdentity identity;
        protected UserContextViewModel userContext;
        protected Guid userId;
        protected Guid? physicianId;
        protected Guid currentContextId;
        protected Guid roleId;
        protected DateTime now;

        public BaseController()
        {
            now = SystemTime.Now();
        }

        string _userName = string.Empty;
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            db = ContextPerRequest.db;
            identity = requestContext.HttpContext.User.Identity;
            userId = User.Identity.GetGuidUserId();
            userContext = User.Identity.GetPhysicianContext();
            physicianId = userContext == null ? (Guid?)null : userContext.Id;
            currentContextId = physicianId.GetValueOrDefault(userId);
            roleId = User.Identity.GetRoleId();
            service = new WorkService(this.db, this.identity);
            accountingService = new AccountingService(this.db, this.identity);
            ViewData["Now"] = now;
        }

        [ChildActionOnly]
        public ActionResult CalendarNavigation(DateTime? selectedDate, CalendarViewOptions? contentView)
        {
            var links = new Dictionary<string, Uri>();
            var date = selectedDate.GetValueOrDefault(now).Date;
            var view = contentView.GetValueOrDefault(CalendarViewOptions.Day);

            links.Add("Previous", Request.Url.AddQuery("SelectedDate", GetPreviousDate(date, view)));
            links.Add("Next", Request.Url.AddQuery("SelectedDate", GetNextDate(date, view)));
            links.Add("Now", Request.Url.AddQuery("SelectedDate", now.ToOrvosiDateFormat()));

            links.Add("Year", Request.Url.AddQuery("ContentView", "Year"));
            links.Add("Month", Request.Url.AddQuery("ContentView", "Month"));
            links.Add("Week", Request.Url.AddQuery("ContentView", "Week"));
            links.Add("Day", Request.Url.AddQuery("ContentView", "Day"));
            links.Add("Today", Request.Url.AddQuery("ContentView", "Day"));

            var viewModel = new CalendarNavigationViewModel
            {
                Links = links,
                ContentView = view,
                SelectedDate = date
            };

            return PartialView(viewModel);
        }

        private string GetPreviousDate(DateTime selectedDate, CalendarViewOptions contentView)
        {
            DateTime result;
            switch (contentView)
            {
                case CalendarViewOptions.Year:
                    result = selectedDate.PreviousYear();
                    break;
                case CalendarViewOptions.Month:
                    result = selectedDate.PreviousMonth();
                    break;
                case CalendarViewOptions.Week:
                    result = selectedDate.FirstDayOfWeek().Previous(DayOfWeek.Sunday);
                    break;
                case CalendarViewOptions.Day:
                    result = selectedDate.AddDays(-1);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return result.ToOrvosiDateFormat();
        }

        private string GetNextDate(DateTime selectedDate, CalendarViewOptions contentView)
        {
            DateTime result;
            switch (contentView)
            {
                case CalendarViewOptions.Year:
                    result = selectedDate.NextYear();
                    break;
                case CalendarViewOptions.Month:
                    result = selectedDate.NextMonth();
                    break;
                case CalendarViewOptions.Week:
                    result = selectedDate.Next(DayOfWeek.Sunday);
                    break;
                case CalendarViewOptions.Day:
                    result = selectedDate.AddDays(1);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return result.ToOrvosiDateFormat();
        }

    }
}