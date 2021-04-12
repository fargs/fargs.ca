using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Company
{
    public class CompanyRole
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public Company Company { get; set; }
    }
}
