using Sample.Common.APIRequest;
using Sample.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Sample.Common.Extentions
{
    public static class HttpContextExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();

        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        public static Task WriteResultAsync<TResult>(this HttpContext context, TResult result)
            where TResult : IActionResult
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = (IActionResultExecutor<TResult>)context.RequestServices.GetService(typeof(IActionResultExecutor<TResult>));

            if (executor == null)
            {
                throw new InvalidOperationException($"No result executor for '{typeof(TResult).FullName}' has been registered.");
            }

            var routeData = context.GetRouteData() ?? EmptyRouteData;

            var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

            return executor.ExecuteAsync(actionContext, result);
        }




        private static APIResult GetResult(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;
            else return JsonConvert.DeserializeObject<APIResult>(content);
        }

        public static T GetResult<T>(this RestResponse<T> restResponse)
        {
            if (restResponse.IsSuccessful)
                return restResponse.Data;
            var rs = GetResult(restResponse.Content);

            if (restResponse.StatusCode == System.Net.HttpStatusCode.BadGateway)
                throw new BadRequestException(rs.UserFriendlyMessage, rs.SystemMessage, rs);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new ForbiddenException(rs.UserFriendlyMessage, rs.SystemMessage, rs);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NotFoundException(rs.UserFriendlyMessage, rs.SystemMessage, rs);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedException(rs.UserFriendlyMessage, rs.SystemMessage, rs);
            throw new BusinessException(rs.UserFriendlyMessage, rs.SystemMessage, rs);
        }
    }
}
