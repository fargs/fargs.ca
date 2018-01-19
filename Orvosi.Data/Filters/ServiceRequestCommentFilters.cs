using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class ServiceRequestCommentFilters
    {
        public static Expression<Func<ServiceRequestComment, bool>> CanAccess(Guid userId)
        {
            return c => c.ServiceRequestCommentAccesses.Select(a => a.AspNetUser.Id).Contains(userId) || c.AspNetUser.Id == userId || c.ServiceRequest.PhysicianId == userId;
        }
        public static IQueryable<ServiceRequestComment> CanAccess(this IQueryable<ServiceRequestComment> serviceRequestComments, Guid userId)
        {
            return serviceRequestComments
                .Where(CanAccess(userId));
        }
    }
}
