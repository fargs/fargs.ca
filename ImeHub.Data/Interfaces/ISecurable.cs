using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Data
{
    public interface ISecurable
    {
        Guid PhysicianId { get; set; }
    }

    public static class ITenantEntityFilters
    {
        public static IQueryable<ISecurable> ForPhysician(this IQueryable<ISecurable> entities, Guid physicianId)
        {
            return entities.Where(c => c.PhysicianId == physicianId);
        }
    }
}
