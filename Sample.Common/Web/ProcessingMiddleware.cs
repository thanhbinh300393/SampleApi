using Sample.Common.Processing;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Sample.Common.Web
{
    public class ProcessingMiddleware
    {

        private readonly RequestDelegate _next;

        public ProcessingMiddleware(
            RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await this._next.Invoke(context);

            ICommandsScheduler commandsScheduler = (ICommandsScheduler)context.RequestServices.GetService(typeof(ICommandsScheduler));
            await commandsScheduler.EnqueueOutBoxToDbAsync();
        }
    }
}