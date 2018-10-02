using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data.Filters
{
    public static class CollaboratorFilters
    {
        public static IQueryable<Collaborator> ForPhysician(this IQueryable<Collaborator> collaborators, Guid physicianId)
        {
            return collaborators.Where(c => c.UserId == physicianId);
        }
    }
}
