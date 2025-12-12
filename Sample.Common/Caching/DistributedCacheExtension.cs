using System;
using Sample.Common.UserSessions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Sample.Common.Caching
{
    public static class DistributedCacheExtension
    {
        public static UserInfo GetUserInfo(this IDistributedCache distributedCache, string accessToken)
        {
            var userInfoJson = distributedCache.GetString($"{KeyCacheConstants.KEY_TOKEN}.{accessToken}");
            if (string.IsNullOrWhiteSpace(userInfoJson))
                return null;
            return JsonConvert.DeserializeObject<UserInfo>(userInfoJson);
        }

        public static void SetUserInfo(this IDistributedCache distributedCache, string accessToken, UserInfo userInfo)
        {
            distributedCache.SetString($"{KeyCacheConstants.KEY_TOKEN}.{accessToken}", JsonConvert.SerializeObject(userInfo), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = userInfo.ExpiryDate - DateTime.Now
            });
        }

        public static bool IsLoggedOut(this IDistributedCache distributedCache, string accessToken)
        {
            var valueLogout = distributedCache.GetString($"{KeyCacheConstants.KEY_TOKEN_LOGGEDOUT}.{accessToken}");
            return !string.IsNullOrWhiteSpace(valueLogout);
        }
    }
}
