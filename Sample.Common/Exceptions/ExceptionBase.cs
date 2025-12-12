using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public abstract class ExceptionBase : Exception, IExceptionBase
    {
        public object? DataTranfer { get; set; }
        public string UserFriendlyMessage { get; set; }
        public virtual HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

        public ExceptionBase(string userFriendlyMessage, string message, object? data = null, Exception? innerException = null)
            : base(message ?? innerException?.Message ?? userFriendlyMessage, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
            DataTranfer = data;
        }
    }
}
