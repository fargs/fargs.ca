using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orvosi.Shared
{
    public static partial class Extensions
    {
        public static DateTime ToLocalTimeZone(this DateTime timeUtc, string timeZone)
        {
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
        }
    }
}
