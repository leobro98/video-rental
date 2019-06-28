using System;

namespace Leobro.VideoStore
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException()
            : base("Customer with this ID not found")
        { }

        public CustomerNotFoundException(string message)
            : base(message)
        { }

        public CustomerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
