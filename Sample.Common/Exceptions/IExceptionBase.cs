using System.Net;

namespace Sample.Common.Exceptions
{
    public interface IExceptionBase
    {
        public HttpStatusCode HttpStatusCode { get; }
        public object? DataTranfer { get; }
        public string UserFriendlyMessage { get; }
        public string Message { get; }
    }
}
