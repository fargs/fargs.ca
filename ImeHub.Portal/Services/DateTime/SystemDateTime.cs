using System;
namespace ImeHub.Portal.Services.DateTimeService
{
    public class SystemDateTime : IDateTime
    {
        public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

        public DateTimeOffset NowOffset => DateTimeOffset.Now;

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime Now => DateTime.Now;
    }
}
