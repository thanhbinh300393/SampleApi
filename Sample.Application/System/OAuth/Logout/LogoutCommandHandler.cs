using Microsoft.Extensions.Caching.Memory;
using Sample.Common.APIRequest;
using Sample.Common.Caching;
using Sample.Common.CQRS.Commands;
using Sample.Domain.Resources;

namespace Sample.Application.System.Users.OAuth.Logout
{
    public class LogoutCommandHandler : CommandHandlerBase<LogoutCommand, object>
    {
        private readonly IMemoryCache _memoryCache;

        public LogoutCommandHandler(
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _memoryCache = memoryCache;
        }

        public override async Task<object> CommandHandle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var token = _userSession.UserInfo?.Token;
            var result = _memoryCache.GetOrCreate<bool>($"{KeyCacheConstants.KEY_TOKEN_LOGGEDOUT}.{token}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                return true;
            });
            return new APIResult() { UserFriendlyMessage = LanguageResource.LogoutSuccess };
        }
    }
}
