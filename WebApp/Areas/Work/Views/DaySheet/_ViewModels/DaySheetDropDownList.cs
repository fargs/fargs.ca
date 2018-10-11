using LinqKit;
using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Library.Extensions;
using WebApp.Models;
using WebApp.Views.Shared;
using FluentDateTime;
using System.Security.Principal;

namespace WebApp.Areas.Work.Views.DaySheet
{
    public class DaySheetDropDownList : ViewModelBase
    {
        public DaySheetDropDownList(OrvosiDbContext db, DateTime selectedMonth, DateTime selectedDate, IIdentity identity, DateTime now) : base(identity, now)
        {
            SelectedDate = selectedDate.ToOrvosiDateFormat();
            SelectedMonth = selectedMonth.ToString("MMMM yyyy");
            Today = now.ToOrvosiDateFormat();

            var startDate = selectedMonth.FirstDayOfMonth();
            var endDate = selectedMonth.LastDayOfMonth();
            var serviceRequests = db.ServiceRequests
                    .AsNoTracking()
                    .AsExpandable()
                    .AreScheduledBetween(startDate, endDate)
                    .AreNotCancellations()
                    .CanAccess(LoggedInUserId, PhysicianId, LoggedInRoleId)
                    .Select(sr => sr.AppointmentDate)
                    .Distinct()
                    .OrderBy(sr => sr)
                    .AsEnumerable();

            DaySheets = serviceRequests
                .Select(sr => new SelectListItem
                {
                    Text = sr.Value.ToString("dd dddd"),
                    Value = sr.ToOrvosiDateFormat()
                });
        }
        public IEnumerable<SelectListItem> DaySheets;
        public string SelectedMonth { get; private set; }
        public string SelectedDate { get; private set; }
        public string Today { get; set; }

    }
}