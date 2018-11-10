using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Data.Interfaces
{
    public interface ILookupEntity<T>
    {
        T Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        string ColorCode { get; set; }
    }
}
namespace ImeHub.Data
{
    public partial class Role : Interfaces.ILookupEntity<Guid>
    {
    }
}
