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
        public static IQueryable<ServiceRequest> AreScheduledThisDay(this IQueryable<ServiceRequest> serviceRequests, DateTime day)
        {
            var endOfDay = day.Date.AddDays(1);
            return serviceRequests
                    .Where(d => d.AppointmentDate.HasValue
                        && d.AppointmentDate.Value >= day.Date && d.AppointmentDate.Value < endOfDay);
        }
        public static IQueryable<ServiceRequest> AreScheduledOnOrBefore(this IQueryable<ServiceRequest> serviceRequests, DateTime now)
        {
            return serviceRequests.Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate : s.DueDate) <= now.Date); // this filters out the days
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
    }
}
