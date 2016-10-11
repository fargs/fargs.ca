using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;

        public static void Reset()
        {
            Now = () => DateTime.Now;
            UtcNow = () => DateTime.UtcNow;
        }
    }
}