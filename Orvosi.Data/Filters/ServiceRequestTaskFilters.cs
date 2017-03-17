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
                .Where(srt => !srt.CompletedDate.HasValue && !srt.IsObsolete && srt.TaskId != Tasks.AssessmentDay);
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
    }
}
