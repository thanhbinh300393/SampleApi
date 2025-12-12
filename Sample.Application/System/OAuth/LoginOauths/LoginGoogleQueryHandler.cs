using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Sample.Application.System.Users.Dto;
using Sample.Common.Caching;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.FilterList;
using Sample.Common.Heplers;
using Sample.Common.UserSessions;
using Sample.Domain.System.Oauth;
using Sample.Domain.System.Users;


namespace Sample.Application.System.OAuth.LoginOauths
{
    public class LoginGoogleQueryHandler : QueryHandlerBase<LoginGoogleQuery, UserInfo>
    {
        private readonly IDapperRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IDistributedCache _distributedCache;
        private readonly ISequenceProvider _sequenceProvider;
        private readonly IDapperRepository<DapperUser> _dapperUserRepository;

        public LoginGoogleQueryHandler(
            IJwtTokenProvider jwtTokenProvider,
            IDistributedCache distributedCache,
            UserManager<User> userManager,
            ISequenceProvider sequenceProvider,
            IDapperRepository<DapperUser> dapperUserRepository,
            IDapperRepository<User> userRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _distributedCache = distributedCache;
            _sequenceProvider = sequenceProvider;
            _userManager = userManager;
            _dapperUserRepository = dapperUserRepository;
        }

        public override async Task<UserInfo> QueryHandle(LoginGoogleQuery request, CancellationToken cancellationToken)
        {
            var googleUser = await GoogleJsonWebSignature.ValidateAsync(request.GoogleToken);
            var googleUserExpire = DateTimeOffset.FromUnixTimeSeconds(googleUser.ExpirationTimeSeconds ?? 0);
            if (googleUser != null && googleUserExpire.DateTime > DateTime.Now.ToUniversalTime())
            {
                var checkUser = await _userRepository.FirstOrDefault(new FilterRequest().Filter("email", googleUser.Email));
                if (checkUser != null)
                {
                    if (!checkUser.IsActive)
                        throw new UnauthorizedException($"Tài khoản của bạn đã bị khóa. Vui lòng liên hệ trung tâm để được hỗ trợ!");

                    var userInfo = _jwtTokenProvider.GetToken(checkUser.GetUserInfo(), false);
                    _distributedCache.SetUserInfo($"Bearer {userInfo.Token}", userInfo);
                    return userInfo;
                }
                else
                {
                   
                    var user = new User()
                    {
                        UserName = googleUser.Email,
                        FullName = googleUser.Name,
                        IsActive = true,
                        IsSystem = false,
                        Email = googleUser.Email,
                        CreatedDate = DateTime.Now,
                        CreatedBy = Enum.GetName(typeof(OauthUserEnum), OauthUserEnum.Google)
                    };
                    var passwordDefault = _configuration["PasswordDefault:password"];
                    await _userManager.CreateAsync(user, passwordDefault ?? "Buca@19006419");

                    // dùng thằng _userManager nên hiện tại phải làm kiểu này
                    var userDapper = MapHelper.Mapper<User, DapperUser>(user);
                    await _dapperUserRepository.UpdateAsync(userDapper);

                    var userInfo = _jwtTokenProvider.GetToken(user.GetUserInfo(), false);
                    _distributedCache.SetUserInfo($"Bearer {userInfo.Token}", userInfo);
                    return userInfo;
                }
            }

            throw new BusinessException("Tài khoản không chính xác, vui lòng kiểm tra lại", "Token error", request);
        }
    }
}