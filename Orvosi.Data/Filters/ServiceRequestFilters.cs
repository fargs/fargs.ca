using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class ServiceRequestFilters
    {
        public static IQueryable<ServiceRequest> WithId(this IQueryable<ServiceRequest> serviceRequests, int id)
        {
            return serviceRequests.Where(sr => sr.Id == id);
        }
        public static IQueryable<ServiceRequest> AreScheduledThisDay(this IQueryable<ServiceRequest> serviceRequests, DateTime day)
        {
            var endOfDay = day.Date.AddDays(1);
            return serviceRequests
                    .Where(d => d.AppointmentDate.HasValue
                        && d.AppointmentDate.Value >= day && d.AppointmentDate.Value < endOfDay);
        }
        public static IQueryable<ServiceRequest> AreScheduledOnOrBefore(this IQueryable<ServiceRequest> serviceRequests, DateTime now)
        {
            now = now.Date.AddDays(1);
            return serviceRequests.Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate.Value : s.DueDate.Value) <= now); // this filters out the days
        }
        public static IQueryable<ServiceRequest> ForPhysician(this IQueryable<ServiceRequest> serviceRequests, Guid userId)
        {
            return serviceRequests.Where(sr => sr.PhysicianId == userId);
        }
        public static IQueryable<ServiceRequest> AreAssignedToUser(this IQueryable<ServiceRequest> serviceRequests, Guid userId)
        {
            return serviceRequests.Where(sr => sr.ServiceRequestTasks.Any(srt => srt.AssignedTo == userId));
        }
        public static IQueryable<ServiceRequest> AreOpen(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests.Where(sr => !sr.IsClosed);
        }
        public static IQueryable<ServiceRequest> AreAddOns(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests.Where(sr => sr.Service.ServiceCategoryId == ServiceCategories.AddOn);
        }
        public static IQueryable<ServiceRequest> HasDueDate(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests.Where(sr => sr.DueDate.HasValue);
        }
        public static IQueryable<ServiceRequest> HaveCompletedSubmitInvoiceTask(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && srt.CompletedDate.HasValue));
        }
        public static IQueryable<ServiceRequest> HaveNotCompletedSubmitInvoiceTask(this IQueryable<ServiceRequest> serviceRequests)
        {
            // where there is no invoice yet and the 
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && !srt.CompletedDate.HasValue && !srt.IsObsolete));
        }

        public static IQueryable<ServiceRequest> AreNotCancellations(this IQueryable<ServiceRequest> serviceRequest)
        {
            return serviceRequest.Where(sr => sr.IsLateCancellation ? true : !sr.CancelledDate.HasValue);
        }

        public static IQueryable<ServiceRequest> AreForCompany(this IQueryable<ServiceRequest> serviceRequests, Guid? companyId)
        {
            if (companyId.HasValue)
            {
                return serviceRequests.Where(sr => sr.Company != null && sr.Company.ObjectGuid == companyId);
            }
            return serviceRequests;
        }

        public static IQueryable<ServiceRequest> AreWithinDateRange(this IQueryable<ServiceRequest> serviceRequests, DateTime now, int? year, int? month)
        {
            if (!year.HasValue && !month.HasValue)
            {
                return serviceRequests.AreScheduledOnOrBefore(now);
            }

            if (year.HasValue)
            {
                serviceRequests = serviceRequests.Where(sr => (sr.AppointmentDate.HasValue ? sr.AppointmentDate.Value.Year : sr.DueDate.HasValue ? sr.DueDate.Value.Year : 0) == year);
            }
            // Apply the year and month filters.
            if (month.HasValue)
            {
                serviceRequests = serviceRequests.Where(sr => (sr.AppointmentDate.HasValue ? sr.AppointmentDate.Value.Month : sr.DueDate.HasValue ? sr.DueDate.Value.Month : 0) == month.Value);
            }

            var addOns = serviceRequests
                .AreAddOns();

            return serviceRequests.Concat(addOns);           
        }
    }
}
