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

        public RefreshTokenQueryHandler(
            IDapperRepository<User> userRepository,
            IJwtTokenProvider jwtTokenProvider,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public override async Task<UserInfo> QueryHandle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var userInfo = _userSession.UserInfo;
            if (userInfo == null)
                throw new UnauthorizedException(LanguageResource.UserUnknown);

            var user = await _userRepository.GetAsync(userInfo.UserId);
            if (user == null)
                throw new UnauthorizedException(LanguageResource.UserUnknown);
            return _jwtTokenProvider.GetToken(user.GetUserInfo(), false);
        }
    }
}
