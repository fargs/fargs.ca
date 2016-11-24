using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Shared.Filters
{
    public static class ServiceRequestTaskFilters
    {
        public static IEnumerable<ServiceRequestTask> AreActive(this IEnumerable<ServiceRequestTask> serviceRequestTasks)
        {
            return serviceRequestTasks
                .Where(srt => !srt.CompletedDate.HasValue && !srt.IsObsolete);
        }
        public static IEnumerable<ServiceRequestTask> AreAssignedToUser(this IEnumerable<ServiceRequestTask> serviceRequestTasks, Guid userId)
        {
            return serviceRequestTasks
                .Where(srt => srt.AssignedTo.Id == userId);
        }
    }
}
