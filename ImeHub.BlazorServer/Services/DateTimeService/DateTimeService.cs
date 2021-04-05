using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.BlazorServer.Services.DateTimeService
{
    public class DateTimeProvider
    {
        public Func<DateTime> Now = () => DateTime.Now;
        public Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}
