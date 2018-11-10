using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImeHub.Data.Interfaces;

namespace ImeHub.Data.Interfaces
{

    public interface IPerson
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string ColorCode { get; set; }
    }
}

namespace ImeHub.Data
{
    public partial class User : Interfaces.IPerson
    {
    }
}
