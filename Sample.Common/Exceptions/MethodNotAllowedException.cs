using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class MethodNotAllowedException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Unauthorized;

        public MethodNotAllowedException(string userFriendlyMessage, string message, object data, Exception innerException = null)
            : base(userFriendlyMessage, message, data, innerException)
        {
        }
    }
}
