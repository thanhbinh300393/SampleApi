using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

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
            var httpContext = _httpContextAccessor.HttpContext;

            var requestId = httpContext?.Items["RequestId"] as string
                ?? Guid.NewGuid().ToString();

            httpContext?.Items.TryAdd("RequestId", requestId);

            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("RequestId", requestId)
            );
        }
    }
}
