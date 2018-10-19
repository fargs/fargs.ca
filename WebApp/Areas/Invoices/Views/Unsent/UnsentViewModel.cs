using Orvosi.Data;
using Orvosi.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApp.Views.Shared;
using FluentDateTime;
using WebApp.Models;
using LinqKit;

namespace WebApp.Areas.Invoices.Views.Unsent
{
    public class UnsentViewModel : ViewModelBase
    {
        public UnsentViewModel(OrvosiDbContext db, DateTime selectedDate, IIdentity identity, DateTime now) : base(identity, now)
        {
            if (!PhysicianId.HasValue)
                throw new PhysicianNullException();

            var startDate = selectedDate.FirstDayOfMonth();
            var endDate = selectedDate.LastDayOfMonth();
            var serviceRequests = db.ServiceRequests
                .ForPhysician(PhysicianId.Value)
                .AreNotCancellations()
                .AreScheduledBetween(startDate, endDate)
                .Select(ServiceRequestDto.FromServiceRequestEntityForInvoice.Expand())
                .ToList();

            var invoices = db.Invoices
                .AreOwnedBy(PhysicianId.Value)
                .AreNotDeleted()
                .AreNotSent()
                .AreInvoicedBetween(startDate, endDate)
                .Select(InvoiceDto.FromInvoiceEntity.Expand())
                .ToList();

            // Full outer join on these 2 lists.
            var leftSide = from sr in serviceRequests
                           join i in invoices on sr.Id equals i.ServiceRequestId into g
                           from sub in g.DefaultIfEmpty()
                           select new UnsentInvoiceViewModel(sr, sub, identity, now);

            var rightSide = from i in invoices
                            where !i.ServiceRequestId.HasValue
                            select new UnsentInvoiceViewModel(null, i, identity, now);

            var result = leftSide.Concat(rightSide).ToList();

            var days = result
                .GroupBy(d => new { Day = d.Day.Date })
                .Select(d => new DayViewModel()
                {
                    Day = d.Key.Day,
                    DayName = d.Key.Day.ToString("MMM dd dddd"),
                    UnsentInvoices = d
                        .OrderBy(sr => d.Key.Day)
                        .ThenBy(sr => sr.ServiceRequest != null && sr.ServiceRequest.StartTime.HasValue ? sr.ServiceRequest.StartTime.Value.Ticks : d.Key.Day.Ticks)
                }).OrderBy(df => df.Day);

            Days = days;
            UnsentInvoiceCount = Days.Sum(day => day.UnsentInvoices.Count());
        }
        public IOrderedEnumerable<DayViewModel> Days { get; set; }
        public int UnsentInvoiceCount { get; set; }
    }
}