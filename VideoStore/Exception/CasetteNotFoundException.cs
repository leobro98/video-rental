using System;

namespace Leobro.VideoStore
{
    public class CasetteNotFoundException : Exception
    {
        public CasetteNotFoundException()
            : base("Casette for this title is not available")
        { }

        public CasetteNotFoundException(string message)
            : base(message)
        { }

        public CasetteNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
