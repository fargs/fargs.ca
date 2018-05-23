using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class TeleconferenceFilters
    {
        public static IQueryable<Teleconference> CanAccess(this IQueryable<Teleconference> query, Guid userId, Guid? physicianId, Guid roleId)
        {
            if (roleId == AspNetRoles.Physician) // physicians should see all there cases
            {
                query = query.Where(t => t.ServiceRequest.PhysicianId == userId);
            }
            else if (physicianId.HasValue) // users that have selected a physician context see all the physician cases
            {
                query = query.Where(t => t.ServiceRequest.PhysicianId == physicianId.Value);
            }
            else if (roleId == AspNetRoles.SuperAdmin)
            {
                return query;
            }
            else// non physician users see cases where tasks are assigned to them
            {
                query = query.Where(t => t.Id == Guid.Empty);
            }

            return query;
        }
    }
}
