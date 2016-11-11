using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class ServiceRequestTaskFilters
    {
        public static IQueryable<ServiceRequestTask> AreActive(this IQueryable<ServiceRequestTask> serviceRequestTasks)
        {
            return serviceRequestTasks
                    .Where(srt => !srt.CompletedDate.HasValue && !srt.IsObsolete);
        }
    }
}
