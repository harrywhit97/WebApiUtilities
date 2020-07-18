using System;

namespace WebApiUtilities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
           : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" was not found with id: {key}")
        {
        }
    }
}
