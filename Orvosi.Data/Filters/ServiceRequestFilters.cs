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

    }
}
