using Moq;
using System;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Testing
{
    public static class Utils
    {
        public static Mock<IClock> GetMockDateTime(DateTimeOffset dateTime)
        {
            var dateTimeMock = new Mock<IClock>();
            dateTimeMock.Setup(x => x.Now)
                .Returns(dateTime);
            return dateTimeMock;
        }
    }
}
