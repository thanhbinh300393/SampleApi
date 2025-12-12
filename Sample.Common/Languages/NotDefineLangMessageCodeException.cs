using System;

namespace Sample.Common.Languages
{
    public class NotDefineLangMessageCodeException : Exception
    {
        public NotDefineLangMessageCodeException(string key, Exception ex) : base($"Not define code {key}", ex)
        {
        }
    }
}
