using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class NotFoundException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;

        public NotFoundException(string userFriendlyMessage, string message, object data, Exception innerException = null)
            : base(userFriendlyMessage, message, data, innerException)
        {
        }
    }
}
