using Microsoft.Extensions.Caching.Distributed;
using Sample.Common.Caching;
using Sample.Common.CQRS.Commands;
using Sample.Common.UserSessions;

namespace Sample.Application.System.OAuth.SetUserSession
{
    public class SetUserSessionCommandHandler : CommandHandlerBase<SetUserSessionCommand, UserInfo>
    {
        private readonly IDistributedCache _distributedCache;
        public SetUserSessionCommandHandler(IDistributedCache distributedCache, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _distributedCache = distributedCache;
        }
        public override async Task<UserInfo> CommandHandle(SetUserSessionCommand request, CancellationToken cancellationToken)
        {
            var userInfo = _userSession.UserInfo;
            //await _distributedCache.RemoveAsync(userInfo.Token);
            //await _distributedCache.SetStringAsync(userInfo.Token, JsonConvert.SerializeObject(userInfo));
            _distributedCache.SetUserInfo($"{userInfo.Token}", userInfo);
            return await Task.FromResult(userInfo);
        }
    }
}
