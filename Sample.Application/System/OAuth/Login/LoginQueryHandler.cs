using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Sample.Common.Caching;
using Sample.Common.CQRS.Queries;
using Sample.Common.Exceptions;
using Sample.Common.UserSessions;
using Sample.Domain.System.Oauth;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.OAuth.Login
{
    public class LoginQueryHandler : QueryHandlerBase<LoginQuery, UserInfo>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IDistributedCache _distributedCache;

        public LoginQueryHandler(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJwtTokenProvider jwtTokenProvider,
            IServiceProvider serviceProvider,
            IDistributedCache distributedCache) : base(serviceProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
            _distributedCache = distributedCache;
        }

        public override async Task<UserInfo> QueryHandle(LoginQuery request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Data.UserName, request.Data.Password, request.Data.IsRemember, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Data.UserName);
                if (user == null)
                    throw new UnauthorizedException($"Tài khoản {request.Data.UserName} không tồn tại trong hệ thống.");

                var userInfo = _jwtTokenProvider.GetToken(user.GetUserInfo(), request.Data.IsRemember);
                _distributedCache.SetUserInfo($"Bearer {userInfo.Token}", userInfo);
                return userInfo;

            }
            else if (result.IsLockedOut)
                throw new UnauthorizedException($"Tài khoản {request.Data.UserName} đã bị khóa.");
            else if (result.IsNotAllowed)
                throw new UnauthorizedException($"Tài khoản {request.Data.UserName} bị từ chối truy cập hệ thống.");
            else throw new BadRequestException($"Thông tin đăng nhập không hợp lệ");
        }
    }
}
