using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models
{
    public interface ILookupModel<T>
    {
        T Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        string ColorCode { get; set; }
    }
}
