using System;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Concrete
{
    public class TimeService : ITimeService
    {
        public DateTimeOffset Now { get => DateTimeOffset.Now; }
        public DateTimeOffset UtcNow { get => DateTimeOffset.UtcNow; }
    }
}
