using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library.Extensions
{
    public static class IEnumerableExtensions
    {
        public static HtmlString ToCSV(this IEnumerable<string> values)
        {
            var result = new HtmlString("'" + string.Join("','", values) + "'");
            return result;
        }
    }
}