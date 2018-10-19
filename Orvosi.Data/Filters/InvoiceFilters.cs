using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class InvoiceFilters
    {
        public static long GetNextInvoiceNumber(this IQueryable<Invoice> invoices, Guid serviceProviderId)
        {
            var data = invoices
                .Where(i => i.ServiceProviderGuid == serviceProviderId)
                .Select(i => i.InvoiceNumber)
                .ToList();

            return data.Any() ? data.Max(i => long.Parse(i)) + 1 : 1;
        }
        public static IQueryable<Invoice> WithId(this IQueryable<Invoice> invoices, long id)
        {
            return invoices.Where(i => i.Id == id);
        }
        public static IQueryable<Invoice> AreOwnedBy(this IQueryable<Invoice> invoices, Guid userId)
        {
            return invoices.Where(i => i.ServiceProviderGuid == userId);
        }
        public static IQueryable<Invoice> AreSent(this IQueryable<Invoice> invoices)
        {
            // this where clause could be refactored into an expression because it is duped in the Shared.Model Invoice IsSent property and InvoiceDto.
            return invoices
                .Where(i => i.SentDate.HasValue
                || i.InvoiceDetails.Any(id => id.ServiceRequest == null ? false : id.ServiceRequest.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && (srt.TaskStatusId == TaskStatuses.Done || srt.TaskStatusId == TaskStatuses.Archive))));
        }
        public static IQueryable<Invoice> AreNotSent(this IQueryable<Invoice> invoices)
        {
            // where there is no invoice yet and the 
            return invoices
                .Where(i => !i.SentDate.HasValue);
        }
        public static IQueryable<Invoice> ArePaid(this IQueryable<Invoice> invoices)
        {
            // this where clause could be refactored into an expression because it is duped in the Shared.Model Invoice IsSent property.
            return invoices
                .Where(i => i.Total <= i.Receipts.Sum(r => r.Amount));
        }
        public static IQueryable<Invoice> AreUnpaid(this IQueryable<Invoice> invoices)
        {
            // this where clause could be refactored into an expression because it is duped in the Shared.Model Invoice IsSent property.
            return invoices
                .Where(i => i.Total.Value > i.Receipts.Select(r => r.Amount).DefaultIfEmpty(0).Sum());
        }
        public static IQueryable<Invoice> AreNotDeleted(this IQueryable<Invoice> invoices)
        {
            // where there is no invoice yet and the 
            return invoices
                .Where(i => !i.IsDeleted);
        }

        public static IQueryable<Invoice> AreForCustomer(this IQueryable<Invoice> invoices, Guid? customerId)
        {
            if (customerId.HasValue)
            {
                return invoices.Where(i => i.CustomerGuid == customerId.Value);
            }
            return invoices;
        }

        public static IQueryable<Invoice> AreInvoicedBetween(this IQueryable<Invoice> invoices, DateTime startDate, DateTime endDate)
        {
            return invoices.Where(c => c.InvoiceDate >= startDate && c.InvoiceDate < endDate);
        }
        public static IQueryable<Invoice> AreWithinDateRange(this IQueryable<Invoice> invoices, DateTime now, int? year, int? month)
        {
            now = now.Date.AddDays(1);
            DateTime currentYear = new DateTime(now.Year, 01, 01);

            if (!year.HasValue && !month.HasValue)
            {
                return invoices.Where(i => i.InvoiceDate <= now);
            }

            if (year.HasValue)
            {
                currentYear = new DateTime(year.Value, 01, 01);
                invoices = invoices.Where(i => i.InvoiceDate >= currentYear);
            }

            // Apply the year and month filters.
            if (month.HasValue)
            {
                var monthStart = currentYear.AddMonths(month.Value - 1);
                var monthEnd = monthStart.AddMonths(1);
                invoices = invoices.Where(c => c.InvoiceDate >= monthStart && c.InvoiceDate < monthEnd);
            }
            return invoices;
        }
    }
}
