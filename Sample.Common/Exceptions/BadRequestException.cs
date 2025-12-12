using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class BadRequestException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;

        public BadRequestException(string userFriendlyMessage, string message = "", object? data = null, Exception innerException = null)
            : base(userFriendlyMessage, message, data, innerException)
        {
        }
    }
}
