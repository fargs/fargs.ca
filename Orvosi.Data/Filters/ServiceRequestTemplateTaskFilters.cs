using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class ServiceRequestTemplateTaskFilters
    {
        public static IQueryable<ServiceRequestTemplateTask> AreNotDeleted(this IQueryable<ServiceRequestTemplateTask> tasks)
        {
            return tasks.Where(i => !i.IsDeleted);
        }
    }
}
