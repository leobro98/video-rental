using System;

namespace Leobro.VideoStore
{
    public class TitleNotFoundException : Exception
    {
        public TitleNotFoundException()
            : base("Title with this ID not found")
        { }

        public TitleNotFoundException(string message)
            : base(message)
        { }

        public TitleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
