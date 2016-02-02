using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;

        public static void Reset()
        {
            Now = () => DateTime.Now;
        }
    }
}