using System;

namespace WebApiUtilities.Interfaces
{
    public interface IClock
    {
        public DateTimeOffset Now { get; }
    }
}
