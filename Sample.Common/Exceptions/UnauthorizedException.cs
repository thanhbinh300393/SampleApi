using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class UnauthorizedException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Unauthorized;

        public UnauthorizedException(string userFriendlyMessage, string message = null, object data = null, Exception innerException = null)
            : base(userFriendlyMessage, message, data, innerException)
        {
        }
    }
}
