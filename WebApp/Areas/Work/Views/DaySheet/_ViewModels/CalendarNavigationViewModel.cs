using FluentDateTime;
using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Views.Shared;

namespace WebApp.Areas.Work.Views.DaySheet
{
    public class CalendarNavigationViewModel : ViewModelBase
    {
        private OrvosiDbContext db;
        private IEnumerable<DateTime> daySheets;
        public CalendarNavigationViewModel(OrvosiDbContext db, DateTime selectedDate, HttpRequestBase request, IIdentity identity, DateTime now) : base(identity, now)
        {
            this.db = db;

            // this loads the selected month, previous month, and next month.
            LoadDaySheets(db, selectedDate);

            DaySheets = GetDaySheetsOfSelectedMonth(selectedDate);

            SelectedDate = selectedDate.ToOrvosiDateFormat();
            SelectedMonth = selectedDate.ToString("MMMM yyyy");

            PreviousMonth = GetFirstDaySheetOfPreviousMonth(selectedDate).ToOrvosiDateFormat();
            NextMonth = GetFirstDaySheetOfNextMonth(selectedDate).ToOrvosiDateFormat();
            
            Today = now.ToOrvosiDateFormat();

            Links = new Dictionary<string, Uri>();
            Links.Add("Previous", request.Url.AddQuery("SelectedDate", PreviousMonth));
            Links.Add("Next", request.Url.AddQuery("SelectedDate", NextMonth));
        }
        public string SelectedMonth { get; private set; }
        public string PreviousMonth { get; private set; }
        public string NextMonth { get; private set; }
        public Dictionary<string, Uri> Links { get; private set; }
        public string SelectedDate { get; private set; }
        public string Today { get; private set; }
        public IEnumerable<SelectListItem> DaySheets { get; private set; }

        private DateTime GetFirstDaySheetOfPreviousMonth(DateTime selectedDate)
        {
            var day = daySheets
                .FirstOrDefault(ds => ds.FirstDayOfMonth() == selectedDate.PreviousMonth().FirstDayOfMonth());

            return (day == DateTime.MinValue ? selectedDate.PreviousMonth().FirstDayOfMonth() : day);
        }
        private DateTime GetFirstDaySheetOfNextMonth(DateTime selectedDate)
        {
            var day = daySheets
                .FirstOrDefault(ds => ds.FirstDayOfMonth() == selectedDate.NextMonth().FirstDayOfMonth());

            return (day == DateTime.MinValue ? selectedDate.NextMonth().FirstDayOfMonth() : day);
        }
        private IEnumerable<SelectListItem> GetDaySheetsOfSelectedMonth(DateTime selectedDate)
        {
            var days = daySheets
                .Where(ds => ds.FirstDayOfMonth() == selectedDate.FirstDayOfMonth());

            return days
                .Select(d => new SelectListItem
                {
                    Value = d.ToOrvosiDateFormat(),
                    Text = d.ToString("dd dddd")
                });
        }
        private void LoadDaySheets(OrvosiDbContext db, DateTime selectedDate)
        {
            var startDate = selectedDate.PreviousMonth().FirstDayOfMonth();
            var endDate = selectedDate.NextMonth().LastDayOfMonth();
            daySheets = db.ServiceRequests
                    .AsNoTracking()
                    .AsExpandable()
                    .AreScheduledBetween(startDate, endDate)
                    .AreNotCancellations()
                    .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                    .Select(sr => sr.AppointmentDate.Value)
                    .Distinct()
                    .OrderBy(sr => sr)
                    .ToList();
        }
    }
}