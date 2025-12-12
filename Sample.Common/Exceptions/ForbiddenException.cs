using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class ForbiddenException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Forbidden;

        public ForbiddenException(string userFriendlyMessage, string message, object data, Exception innerException = null)
            : base(userFriendlyMessage, message, data, innerException)
        {
        }
    }
}
