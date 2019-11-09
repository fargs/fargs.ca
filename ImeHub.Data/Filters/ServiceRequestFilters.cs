using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Data
{
    public static class ServiceRequestFilters
    {
        public static IQueryable<ServiceRequest> CanAccess(this IQueryable<ServiceRequest> query, Guid userId, Guid? physicianId, Guid roleId)
        {
            if (roleId == Enums.Roles.Physician) // physicians should see all there cases
            {
                query = (IQueryable<ServiceRequest>)((IQueryable<ISecurable>)query).ForPhysician(userId);
            }
            else if (physicianId.HasValue) // users that have selected a physician context see all the physician cases
            {
                query = (IQueryable<ServiceRequest>)((IQueryable<ISecurable>)query).ForPhysician(physicianId.Value);
            }
            else if (roleId == Enums.Roles.SuperAdmin)
            {
                return query;
            }
            else// non physician users see cases where tasks are assigned to them
            {
                query = query.AreAssignedToUser(userId);
            }

            return query;
        }
        public static IQueryable<ServiceRequest> AreAssignedToUser(this IQueryable<ServiceRequest> serviceRequests, Guid userId)
        {
            return serviceRequests.Where(AreAssignedToUser(userId));
        }
        public static Expression<Func<ServiceRequest, bool>> AreAssignedToUser(Guid userId)
        {
            return sr => true;
        }
    }
}
