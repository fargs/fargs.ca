using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImeHub.Data;

namespace ImeHub.Models
{
    public static class Case
    {
        public static string GetNextCaseNumber(this IQueryable<ImeHub.Data.Case> cases, Guid physicianId)
        {
            var data = cases
                .Where(c => c.PhysicianId == physicianId)
                .Select(i => i.CaseNumber)
                .ToList();

            var caseNumber = data.Any() ? data.Max(i => long.Parse(i)) + 1 : 1;
            return caseNumber.ToString();
        }
    }
}
