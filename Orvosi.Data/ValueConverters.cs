using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Data
{
    public static class ValueConverters
    {
        public static string GetDisplayName(string title, string firstName, string lastName)
        {
            return $"{(!string.IsNullOrEmpty(title) ? title + " " : "")}{firstName} {lastName}";
        }
    }
}
