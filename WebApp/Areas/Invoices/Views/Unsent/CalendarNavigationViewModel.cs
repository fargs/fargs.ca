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
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class CalendarNavigationViewModel : ViewModelBase
    {
        public CalendarNavigationViewModel(DateTime selectedDate, OrvosiDbContext db, HttpRequestBase request, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue)
                throw new PhysicianNullException();

            SelectedDate = selectedDate.ToOrvosiDateFormat();
            SelectedMonth = selectedDate.ToString("MMMM yyyy");

            PreviousMonth = selectedDate.PreviousMonth().FirstDayOfMonth().ToOrvosiDateFormat();
            NextMonth = selectedDate.NextMonth().FirstDayOfMonth().ToOrvosiDateFormat();

            ThisMonth = now.FirstDayOfMonth().ToOrvosiDateFormat();

            Links = new Dictionary<string, Uri>();
            Links.Add("Previous", request.Url.AddQuery("SelectedDate", PreviousMonth));
            Links.Add("Next", request.Url.AddQuery("SelectedDate", NextMonth));

            var serviceRequests = db.ServiceRequests
                .AsNoTracking()
                .ForPhysician(PhysicianId.Value)
                .AreNotCancellations()
                .Select(sr => new ServiceRequestDto
                {
                    Id = sr.Id,
                    AppointmentDate = sr.AppointmentDate,
                    DueDate = sr.DueDate
                })
                .ToList();

            var invoices = db.Invoices
                .AreOwnedBy(PhysicianId.Value)
                .AreNotDeleted()
                .AreNotSent()
                .Select(InvoiceDto.FromInvoiceEntity.Expand())
                .ToList();

            // Full outer join on these 2 lists.
            var leftSide = from sr in serviceRequests
                           join i in invoices on sr.Id equals i.ServiceRequestId into g
                           from sub in g.DefaultIfEmpty()
                           select sr.ExpectedInvoiceDate;

            var rightSide = from i in invoices
                            where !i.ServiceRequestId.HasValue
                            select i.InvoiceDate;

            var result = leftSide.Concat(rightSide).ToList();

            Months = new List<MonthViewModel>();
            Months = result
                .GroupBy(d => d.FirstDayOfMonth())
                .Select(d => new MonthViewModel
                {
                    Id = d.Key.ToOrvosiDateFormat(),
                    Text = $"{d.Key.ToString("yyyy MMM")} ({d.Count().ToString()})"
                }).OrderBy(df => df.Id);

        }
        public string SelectedMonth { get; private set; }
        public string PreviousMonth { get; private set; }
        public string NextMonth { get; private set; }
        public Dictionary<string, Uri> Links { get; private set; }
        public string SelectedDate { get; private set; }
        public string ThisMonth { get; private set; }
        public IEnumerable<MonthViewModel> Months { get; set; }
        public class MonthViewModel
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
    }
}