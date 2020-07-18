using System;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Concrete
{
    public class Clock : IClock
    {
        public DateTimeOffset Now { get => DateTimeOffset.Now; }
    }
}
