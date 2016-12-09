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
            // this where clause could be refactored into an expression because it is duped in the Shared.Model Invoice IsSent property.
            return invoices
                .Where(i => i.SentDate.HasValue
                || i.InvoiceDetails.Any(id => id.ServiceRequest == null ? false : id.ServiceRequest.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && srt.CompletedDate.HasValue)));
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

        public static IQueryable<Invoice> AreWithinDateRange(this IQueryable<Invoice> invoices, int year, int? month)
        {
            invoices = invoices.Where(i => i.InvoiceDate.Year == year);
            // Apply the year and month filters.
            if (month.HasValue)
            {
                return invoices.Where(c => c.InvoiceDate.Month == month.Value);
            }

            return invoices;
        }
    }
}
