using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Data.Company
{
    public class Company
    {
        public short Id { get; set; }
        public Guid ObjectGuid { get; set; }
        public string Name { get; set; }
    }
}
