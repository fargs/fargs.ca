using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    public partial class AspNetUser
    {
        public string GetDisplayName()
        {
            return $"{(!string.IsNullOrEmpty(Title) ? Title + " " : "")}{FirstName} {LastName}";
        }

        public string GetInitials()
        {
            return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
        }
    }
}
