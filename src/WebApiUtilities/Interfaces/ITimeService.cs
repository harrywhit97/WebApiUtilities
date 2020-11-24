using System;

namespace WebApiUtilities.Interfaces
{
    public interface ITimeService
    {
        public DateTimeOffset Now { get; }
        public DateTimeOffset UtcNow { get; }
    }
}
