using Sample.Common.Caching;
using Sample.Common.UserSessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

namespace Sample.Common.Web
{
    public class AppAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            IUserSession userSession = (IUserSession)context.HttpContext.RequestServices.GetService(typeof(IUserSession));
            IDistributedCache distributedCache = (IDistributedCache)context.HttpContext.RequestServices.GetService(typeof(IDistributedCache));

            var isLogin = false;
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var accessToken = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (!distributedCache.IsLoggedOut(accessToken))
                {
                    isLogin = true;
                    UserInfo userInfo = distributedCache.GetUserInfo(accessToken);
                    if (userInfo != null)
                    {
                        userInfo.Token = accessToken;
                        userSession.SetUserInfo(userInfo);
                    }
                    else isLogin = false;
                }
            }
            if (!isLogin)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
