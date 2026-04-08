using Microsoft.Extensions.Caching.Distributed;
using Sample.Common.Caching;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.UserSessions;
using Sample.Domain.Resources;
using Sample.Domain.System.Oauth;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.OAuth.RefreshToken
{
    public class RefreshTokenQueryHandler : QueryHandlerBase<RefreshTokenQuery, UserInfo>
    {
        private readonly IDapperRepository<User> _userRepository;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IDistributedCache _distributedCache;

        public RefreshTokenQueryHandler(
            IDapperRepository<User> userRepository,
            IJwtTokenProvider jwtTokenProvider,
            IServiceProvider serviceProvider,
            IDistributedCache distributedCache) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _distributedCache = distributedCache;
        }

        public override async Task<UserInfo> QueryHandle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var userInfo = _distributedCache.GetUserInfo($"Bearer {request.RefreshToken}");
            if (userInfo == null)
                throw new UnauthorizedException("Token hết hạn");

            var user = await _userRepository.GetAsync(userInfo.UserId);
            if (user == null)
                throw new UnauthorizedException(LanguageResource.UserUnknown);
            var newUserInfo = _jwtTokenProvider.GetToken(user.GetUserInfo(), false);
            _distributedCache.SetUserInfo($"Bearer {newUserInfo.Token}", userInfo);
            _distributedCache.SetUserInfo($"Bearer {newUserInfo.RefreshToken}", userInfo);
            return newUserInfo;
        }
    }
}
