using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
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
        public static IQueryable<ServiceRequest> CanAccess(this IQueryable<ServiceRequest> query, Guid userId, Guid? physicianId, Guid roleId)
        {
            if (roleId == AspNetRoles.Physician) // physicians should see all there cases
            {
                query = query.ForPhysician(userId);
            }
            else if (physicianId.HasValue) // users that have selected a physician context see all the physician cases
            {
                query = query.ForPhysician(physicianId.Value);
            }
            else if (roleId == AspNetRoles.SuperAdmin)
            {
                return query;
            }
            else// non physician users see cases where tasks are assigned to them
            {
                query = query.AreAssignedToUser(userId);
            }

            return query;
        }
        public static IQueryable<ServiceRequest> HaveAppointment(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests
                    .Where(d => d.AppointmentDate.HasValue);
        }
        public static IQueryable<ServiceRequest> HaveNoAppointment(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests
                    .Where(d => !d.AppointmentDate.HasValue);
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
            return serviceRequests.Where(s => (s.AppointmentDate.HasValue ? s.AppointmentDate.Value : new DateTime(1900,01,01)) <= now); // this filters out the days
        }
        public static IQueryable<ServiceRequest> AreScheduledBetween(this IQueryable<ServiceRequest> serviceRequests, DateTime startDate, DateTime endDate)
        {
            return serviceRequests
                .Where(AreScheduledBetween(startDate, endDate)); // this filters out the days
        }
        public static Expression<Func<ServiceRequest, bool>> AreScheduledBetween(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1);
            return s => s.AppointmentDate.HasValue && s.AppointmentDate.Value >= startDate && s.AppointmentDate.Value < endDate;
        }
        public static IQueryable<ServiceRequest> ForPhysician(this IQueryable<ServiceRequest> serviceRequests, Guid userId)
        {
            return serviceRequests.Where(sr => sr.PhysicianId == userId);
        }
        public static IQueryable<ServiceRequest> AreAssignedToUser(this IQueryable<ServiceRequest> serviceRequests, Guid userId)
        {
            return serviceRequests.Where(AreAssignedToUser(userId));
        }
        public static Expression<Func<ServiceRequest, bool>> AreAssignedToUser(Guid userId)
        {
            return sr => sr.ServiceRequestTasks.Any(srt => srt.AssignedTo == userId);
        }
        public static IQueryable<ServiceRequest> AreNotClosed(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests.Where(sr => sr.ServiceRequestStatusId != ServiceRequestStatuses.Closed);
        }
        public static IQueryable<ServiceRequest> AreAddOns(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests.Where(sr => sr.Service.ServiceCategoryId == ServiceCategories.AddOn);
        }
        public static IQueryable<ServiceRequest> HasDueDate(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests.Where(sr => sr.DueDate.HasValue);
        }
        public static IQueryable<ServiceRequest> WhereSubmitInvoiceTaskIsToDo(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && srt.TaskStatusId == TaskStatuses.ToDo));
        }
        public static IQueryable<ServiceRequest> HaveCompletedSubmitInvoiceTask(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && (srt.TaskStatusId == TaskStatuses.Done || srt.TaskStatusId == TaskStatuses.Archive)));
        }
        public static IQueryable<ServiceRequest> HaveNotCompletedSubmitInvoiceTask(this IQueryable<ServiceRequest> serviceRequests)
        {
            // where there is no invoice yet and the 
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.SubmitInvoice && (srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting)));
        }
        public static IQueryable<ServiceRequest> HaveCompletedAssessmentDayTask(this IQueryable<ServiceRequest> serviceRequests)
        {
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.AssessmentDay && (srt.TaskStatusId == TaskStatuses.Done || srt.TaskStatusId == TaskStatuses.Archive)));
        }
        public static IQueryable<ServiceRequest> HaveNotCompletedAssessmentDayTask(this IQueryable<ServiceRequest> serviceRequests)
        {
            // where there is no invoice yet and the 
            return serviceRequests
                .Where(sr => sr.ServiceRequestTasks.Any(srt => srt.TaskId == Tasks.AssessmentDay && (srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting)));
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

            return serviceRequests;
        }
    }
}
