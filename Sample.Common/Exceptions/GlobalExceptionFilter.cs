using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Sample.Common.APIRequest;
using Sample.Common.Languages;
using Serilog;
using System.Net;

namespace Sample.Common.Exceptions
{
    public class GlobalExceptionFilter<TResource> : IAsyncExceptionFilter
        where TResource : class
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var L = context.HttpContext.RequestServices.GetService(typeof(ILanguageProvider<TResource>)) as ILanguageProvider<TResource>;
            var _logger = context.HttpContext.RequestServices.GetService(typeof(ILogger)) as ILogger;

            var exception = context.Exception;

            _logger?.Error(exception, exception.Message);

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string userMessage = L?["SomethingIsWrong"] ?? "Something went wrong";

            if (exception is SqlException sqlEx && sqlEx.Number == 547)
                userMessage = L?["NoneBusinessArises"] ?? "Business constraint error";

            // Dùng ObjectResult thay cho JsonResult
            context.Result = new ObjectResult(new APIResult
            {
                Data = null,
                SystemMessage = exception.InnerException?.Message ?? exception.Message,
                UserFriendlyMessage = userMessage
            })
            {
                StatusCode = context.HttpContext.Response.StatusCode
            };

            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }
}
