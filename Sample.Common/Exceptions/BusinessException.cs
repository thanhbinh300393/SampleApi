using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class BusinessException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

        public BusinessException(string userFriendlyMessage, string message, object data, Exception innerException = null)
            : base(userFriendlyMessage, message, data, innerException)
        {
        }
    }
}
