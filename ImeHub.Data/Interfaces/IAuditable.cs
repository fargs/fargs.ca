using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Data
{
    public interface IAuditable
    {
        Guid ModifiedById { get; set; }
        string ModifiedBy { get; set; }

    }
}
