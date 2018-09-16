using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDayOrDefault(this DateTime? date)
        {
            return date.HasValue ? date.Value : SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
        }
    }
}