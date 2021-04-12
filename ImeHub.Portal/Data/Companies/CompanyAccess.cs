using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Company
{
    public class CompanyAccess
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }

        public CompanyRole CompanyRole { get; set; }
    }
}
