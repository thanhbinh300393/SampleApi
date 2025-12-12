using System;
using System.Net;

namespace Sample.Common.Exceptions;

public class TimeoutRequestException : ExceptionBase
{
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.RequestTimeout;

    public TimeoutRequestException(string userFriendlyMessage, string message, object data, Exception innerException = null)
        : base(userFriendlyMessage, message, data, innerException)
    {
    }
}