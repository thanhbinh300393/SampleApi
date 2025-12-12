using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Sample.Common.Logging
{
    public class ThreadIdEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThreadIdEnricher() : this(new HttpContextAccessor())
        {
        }

        public ThreadIdEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string requestId = Guid.NewGuid().ToString(); ;
            if (_httpContextAccessor.HttpContext?.Items.ContainsKey("RequestId") ?? false)
                requestId = _httpContextAccessor.HttpContext.Items["RequestId"].ToString();
            else if (_httpContextAccessor.HttpContext != null)
                _httpContextAccessor.HttpContext.Items.Add("RequestId", requestId);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                    "RequestId", $"{requestId}"));
        }
    }
}
