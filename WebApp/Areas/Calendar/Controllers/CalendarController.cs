using FluentDateTime;
using ImeHub.Data;
using ImeHub.Models;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Availability.Views.Home;
using WebApp.Areas.Calendar.Views.Calendar;
using WebApp.Controllers;
using WebApp.Library.Extensions;
using WebApp.ViewModels.CalendarViewModels;

namespace WebApp.Areas.Calendar.Controllers
{
    public class CalendarController : BaseController
    {
        private ImeHubDbContext context;
        private int lookAheadInDays = 150;

        public CalendarController(ImeHubDbContext context, DateTime now, IPrincipal principal) : base(now, principal)
        {
            this.context = context;
        }

        //#region Public

        //public ActionResult Index()
        //{
        //    var range = GetInitialDateRange(this.now, lookAheadInDays);

        //    var availableDays = GetAvailableDaysWithinRange(range);

        //    var joined = from d in range.DateRange
        //                 join ad in availableDays on d.Date equals ad.Day.Date into dad
        //                 from ad in dad.DefaultIfEmpty()
        //                 select new AvailabilityDate
        //                 {
        //                     Day = d,
        //                     AvailableDay = ad
        //                 };
        //    var weeks = joined.GroupBy(d => d.Day.FirstDayOfWeek()).GroupBy(w => w.Key.FirstDayOfMonth());

        //    var dateRange = new AvailabilityDateRange(weeks, range);

        //    var viewModel = new Index(dateRange);

        //    return View(viewModel);
        //}

        //public ActionResult GetRange(DateTime startDate, bool getPrevious = false)
        //{
        //    DateRangeViewModel range;
        //    if (getPrevious)
        //    {
        //        range = GetPreviousDateRange(startDate, lookAheadInDays);
        //    }
        //    else
        //    {
        //        range = GetNextDateRange(startDate, lookAheadInDays);
        //    }

        //    var availableDays = GetAvailableDaysWithinRange(range);

        //    var joined = from d in range.DateRange
        //                 join ad in availableDays on d.Date equals ad.Day.Date into dad
        //                 from ad in dad.DefaultIfEmpty()
        //                 select new AvailabilityDate
        //                 {
        //                     Day = d,
        //                     AvailableDay = ad
        //                 };
        //    var weeks = joined.GroupBy(d => d.Day.FirstDayOfWeek()).GroupBy(w => w.Key.FirstDayOfMonth());

        //    var viewModel = new AvailabilityDateRange(weeks, range);

        //    return PartialView("AvailabilityDateRange", viewModel);
        //}

        //public ActionResult DayView(DateTime day)
        //{
        //    var availableDay = context.AvailableDays
        //        .Where(c => c.PhysicianId == physicianId)
        //        .Where(ad => ad.Day == day)
        //        .Select(AvailableDayModel.FromAvailableDayEntityForDayView.Expand())
        //        .SingleOrDefault();

        //    var availableDayViewModel = AvailableDayViewModel.FromAvailableDayDto.Invoke(availableDay);
        //    var viewModel = new DayView
        //    {
        //        AvailableDay = availableDayViewModel
        //    };

        //    return PartialView(viewModel);

        //    //// Set date range variables used in where conditions
        //    //var dto = context.ServiceRequests
        //    //    .AsExpandable()
        //    //    .AreScheduledThisDay(day)
        //    //    .AreNotCancellations()
        //    //    .CanAccess(loggedInUserId, physicianId, loggedInRoleId)
        //    //    .Select(ServiceRequestDto.FromServiceRequestEntityForCaseLinks(loggedInUserId))
        //    //    .OrderBy(sr => sr.AppointmentDate).ThenBy(sr => sr.StartTime)
        //    //    .ToList();

        //    //var caseViewModels = dto.AsQueryable()
        //    //    .Select(CaseLinkViewModel.FromServiceRequestDto.Expand());

        //    //var dayViewModel = caseViewModels
        //    //    .GroupBy(c => c.AppointmentDate.Value)
        //    //    .AsQueryable()
        //    //    .Select(DayViewModel.FromServiceRequestDtoGroupingDtoForCaseLinks.Expand())
        //    //    .SingleOrDefault();

        //    //return PartialView(dayViewModel);
        //}

        //#endregion

        #region Private

        private DateRangeViewModel GetInitialDateRange(DateTime startDate, int lookAheadInDays)
        {
            var firstDayOfStartWeek = startDate.FirstDayOfMonth().FirstDayOfWeek();
            var firstDayOfEndWeek = firstDayOfStartWeek.AddDays(lookAheadInDays).LastDayOfMonth().FirstDayOfWeek();
            var lastDayOfEndWeek = firstDayOfEndWeek.LastDayOfWeek();

            var range = firstDayOfStartWeek.GetDateRangeTo(lastDayOfEndWeek);

            return new DateRangeViewModel
            {
                DateRange = range,
                FirstDayOfStartWeek = firstDayOfStartWeek,
                FirstDayOfEndWeek = firstDayOfEndWeek,
                LastDayOfEndWeek = lastDayOfEndWeek
            };
        }

        private DateRangeViewModel GetNextDateRange(DateTime startDate, int lookAheadInDays)
        {
            var firstDayOfStartWeek = startDate.AddDays(7).FirstDayOfWeek();
            var firstDayOfEndWeek = firstDayOfStartWeek.AddDays(lookAheadInDays).LastDayOfMonth().FirstDayOfWeek();
            var lastDayOfEndWeek = firstDayOfEndWeek.LastDayOfWeek(); 

            var range = firstDayOfStartWeek.GetDateRangeTo(lastDayOfEndWeek);

            return new DateRangeViewModel
            {
                DateRange = range,
                FirstDayOfStartWeek = firstDayOfStartWeek,
                FirstDayOfEndWeek = firstDayOfEndWeek,
                LastDayOfEndWeek = lastDayOfEndWeek
            };
        }

        private DateRangeViewModel GetPreviousDateRange(DateTime startDate, int lookAheadInDays)
        {
            var firstDayOfEndWeek = startDate.AddDays(-7).FirstDayOfWeek();
            var lastDayOfEndWeek = firstDayOfEndWeek.LastDayOfWeek(); 
            var firstDayOfStartWeek = firstDayOfEndWeek.AddDays(lookAheadInDays * -1).FirstDayOfMonth().FirstDayOfWeek();

            var range = firstDayOfStartWeek.GetDateRangeTo(lastDayOfEndWeek);

            return new DateRangeViewModel
            {
                DateRange = range,
                FirstDayOfStartWeek = firstDayOfStartWeek,
                FirstDayOfEndWeek = firstDayOfEndWeek,
                LastDayOfEndWeek = lastDayOfEndWeek
            };
        }

        //private IEnumerable<AvailableDayViewModel> GetAvailableDaysWithinRange(DateRangeViewModel range)
        //{
        //    var start = range.FirstDayOfStartWeek;
        //    var end = range.LastDayOfEndWeek;
        //    // Second sequence
        //    List<AvailableDayModel> model;
        //    using (var context = new OrvosiDbContext())
        //    {
        //        model = context.AvailableDays
        //        .Where(ad => ad.Day >= start && ad.Day <= end)
        //        .Where(ad => ad.PhysicianId == new Guid("8E9885D8-A0F7-49F6-9A3E-FF1B4D52F6A9"))
        //        .Select(AvailableDayModel.FromAvailableDayEntity.Expand())
        //        .ToList();
        //    }

        //    var availableDays = model.AsQueryable().Select(AvailableDayViewModel.FromAvailableDayDto.Expand());
        //    return availableDays;
        //}

        #endregion
    }
}