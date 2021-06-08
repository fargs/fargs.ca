using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Data.Companies
{
    public class CompanyAccess
    {
        public long Id { get; set; }
        public Guid ObjectGuid { get; set; }
        public Guid ModifiedBy { get; set; }
        public Guid UserId { get; set; }

        public CompanyRole CompanyRole { get; set; }
    }
}
