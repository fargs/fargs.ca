using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Shared.Filters
{
    public static class ServiceRequestFilters
    {
        public static IEnumerable<ServiceRequest> CanAccess(this IEnumerable<ServiceRequest> serviceRequests, Guid userId)
        {
            // TODO: add AssignedToId to the ServiceRequestTask Model class to avoid the need for the null reference check
            return serviceRequests
                .Where(sr => !sr.ServiceRequestTasks.Any(srt => (srt.AssignedTo == null ? null : srt.AssignedTo.Id) == userId));
        }
    }
}
