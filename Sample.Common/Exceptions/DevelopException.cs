using System;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class DevelopException : ExceptionBase
    {
        public override HttpStatusCode HttpStatusCode => HttpStatusCode.InternalServerError;

        public DevelopException(string message, object data, Exception innerException = null)
            : base("Lỗi kỹ thuật, vui lòng liên hệ quản trị viên!", message, data, innerException)
        {
        }
    }
}
