using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public static class ServiceRequestDtoFilters
    {
        public static IQueryable<ServiceRequestDto> AreScheduledBetween(this IQueryable<ServiceRequestDto> serviceRequests, DateTime startDate, DateTime endDate)
        {
            return serviceRequests
                .Where(AreScheduledBetween(startDate, endDate)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestDto, bool>> AreScheduledBetween(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1);
            return s => s.AppointmentDate.HasValue && s.AppointmentDate.Value >= startDate && s.AppointmentDate.Value < endDate;
        }
        public static IQueryable<ServiceRequestDto> AreDueBetween(this IQueryable<ServiceRequestDto> serviceRequests, DateTime startDate, DateTime endDate)
        {
            return serviceRequests
                .Where(AreDueBetween(startDate, endDate)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestDto, bool>> AreDueBetween(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1);
            return s => s.DueDate.HasValue && s.DueDate.Value >= startDate && s.DueDate.Value < endDate;
        }
    }
}