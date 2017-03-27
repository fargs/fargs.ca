using LinqKit;
using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class ServiceRequestTaskFilters
    {
        public static IQueryable<ServiceRequestTask> AreActive(this IQueryable<ServiceRequestTask> serviceRequestTasks)
        {
            return serviceRequestTasks
                .Where(srt => srt.TaskStatusId == TaskStatuses.ToDo || srt.TaskStatusId == TaskStatuses.Waiting || srt.TaskStatusId == TaskStatuses.OnHold);
        }
        public static IQueryable<ServiceRequestTask> AreAssignedToUser(this IQueryable<ServiceRequestTask> serviceRequestTasks, Guid userId)
        {
            return serviceRequestTasks
                .Where(AreAssignedToUser(userId));
        }
        public static Expression<Func<ServiceRequestTask, bool>> AreAssignedToUser(Guid userId)
        {
            return srt => srt.AssignedTo == userId;
        }
        public static IQueryable<ServiceRequestTask> WithTaskId(this IQueryable<ServiceRequestTask> serviceRequestTasks, short taskId)
        {
            return serviceRequestTasks
                .Where(WithTaskId(taskId)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestTask, bool>> WithTaskId(short taskId)
        {
            return s => s.TaskId == taskId;
        }
        public static IQueryable<ServiceRequestTask> WithTaskIds(this IQueryable<ServiceRequestTask> srt, short[] taskIds)
        {
            return srt.Where(WithTaskIds(taskIds));
        }
        public static Expression<Func<ServiceRequestTask, bool>> WithTaskIds(short[] taskIds)
        {
            var predicate = PredicateBuilder.New<ServiceRequestTask>(true);
            if (taskIds == null)
            {
                return predicate;
            }
            foreach (var id in taskIds)
            {
                predicate = predicate.Or(srt => srt.TaskId == id);
            }
            return predicate;
        }

        public static IQueryable<ServiceRequestTask> WithServiceRequestId(this IQueryable<ServiceRequestTask> serviceRequestTasks, int serviceRequestId)
        {
            return serviceRequestTasks
                .Where(WithServiceRequestId(serviceRequestId)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestTask, bool>> WithServiceRequestId(int serviceRequestId)
        {
            return s => s.ServiceRequestId == serviceRequestId;
        }
        public static IQueryable<ServiceRequestTask> AreOnCriticalPath(this IQueryable<ServiceRequestTask> serviceRequestTasks)
        {
            return serviceRequestTasks
                .Where(AreOnCriticalPath());
        }
        public static Expression<Func<ServiceRequestTask, bool>> AreOnCriticalPath()
        {
            return srt => srt.IsCriticalPath == true;
        }

        public static IQueryable<ServiceRequestTask> AreDueBetween(this IQueryable<ServiceRequestTask> serviceRequestTasks, DateTime startDate, DateTime endDate)
        {
            return serviceRequestTasks
                .Where(AreDueBetween(startDate, endDate)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestTask, bool>> AreDueBetween(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1);
            return s => s.DueDate.HasValue && s.DueDate.Value >= startDate && s.DueDate.Value < endDate;
        }
        public static IQueryable<ServiceRequestTask> AreScheduledBetween(this IQueryable<ServiceRequestTask> serviceRequestTasks, DateTime startDate, DateTime endDate)
        {
            return serviceRequestTasks
                .Where(AreScheduledBetween(startDate, endDate)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestTask, bool>> AreScheduledBetween(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1);
            return s => s.ServiceRequest.AppointmentDate.HasValue && s.ServiceRequest.AppointmentDate.Value >= startDate && s.ServiceRequest.AppointmentDate.Value < endDate;
        }
        public static IQueryable<ServiceRequestTask> ThatAreOverdue(this IQueryable<ServiceRequestTask> serviceRequestTasks, DateTime now)
        {
            return serviceRequestTasks
                .WithTaskStatus(TaskStatuses.ToDo, TaskStatuses.Waiting, TaskStatuses.OnHold)
                .Where(ThatAreOverdue(now)); // this filters out the days
        }
        public static IQueryable<ServiceRequestTask> ThatAreOverdue(this IQueryable<ServiceRequestTask> serviceRequestTasks, DateTime now, short taskId)
        {
            return serviceRequestTasks
                .Where(WithTaskId(taskId))
                .Where(ThatAreOverdue(now)); // this filters out the days
        }
        public static Expression<Func<ServiceRequestTask, bool>> ThatAreOverdue(DateTime now)
        {
            return s => s.DueDate.HasValue && s.DueDate.Value < now;
        }

        public static IQueryable<ServiceRequestTask> WithTaskStatus(this IQueryable<ServiceRequestTask> serviceRequestTasks, params short[] taskStatuses)
        {
            return serviceRequestTasks
                .Where(WithTaskStatus(taskStatuses).Expand());
        }
        public static Expression<Func<ServiceRequestTask, bool>> WithTaskStatus(params short[] taskStatuses)
        {
            var predicate = PredicateBuilder.New<ServiceRequestTask>();
            foreach (var taskStatusId in taskStatuses)
            {
                predicate = predicate.Or(t => t.TaskStatusId == taskStatusId);
            }
            return predicate;
        }
    }
}
