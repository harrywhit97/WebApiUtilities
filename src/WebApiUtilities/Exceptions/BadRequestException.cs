using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUtilities.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException()
           : base()
        {
        }

        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
