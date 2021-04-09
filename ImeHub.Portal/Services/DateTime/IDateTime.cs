using System;

namespace ImeHub.Portal.Services.DateTimeService
{
    public interface IDateTime
    {
        DateTimeOffset UtcNowOffset { get; }
        DateTimeOffset NowOffset { get; }
        DateTime UtcNow { get; }
        DateTime Now { get; }
    }
}
