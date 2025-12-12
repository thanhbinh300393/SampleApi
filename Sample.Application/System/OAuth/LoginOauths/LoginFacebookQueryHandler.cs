using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Sample.Application.System.Users.Dto;
using Sample.Common.Caching;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Extentions;
using Sample.Common.FilterList;
using Sample.Common.Heplers;
using Sample.Common.UserSessions;
using Sample.Domain.System.Oauth;
using Sample.Domain.System.Users;

namespace Sample.Application.System.OAuth.LoginOauths
{
    public class LoginFacebookQueryHandler : QueryHandlerBase<LoginFacebookQuery, UserInfo>
    {
        private readonly IDapperRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IDistributedCache _distributedCache;
        private readonly ISequenceProvider _sequenceProvider;
        private readonly IDapperRepository<DapperUser> _dapperUserRepository;
        public LoginFacebookQueryHandler(
            IJwtTokenProvider jwtTokenProvider,
            IDistributedCache distributedCache,
            UserManager<User> userManager,
            ISequenceProvider sequenceProvider,
            IDapperRepository<DapperUser> dapperUserRepository,
            IDapperRepository<User> userRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _jwtTokenProvider = jwtTokenProvider;
            _distributedCache = distributedCache;
            _sequenceProvider = sequenceProvider;
            _userManager = userManager;
            _dapperUserRepository = dapperUserRepository;
        }

        public override async Task<UserInfo> QueryHandle(LoginFacebookQuery request, CancellationToken cancellationToken)
        {
            string appId = _configuration["FacebookApp:FacebookAppId"];
            string appSecret = _configuration["FacebookApp:FacebookAppSecret"];
            string appAccessToken = $"{appId}|{appSecret}";

            string validationUrl = $"https://graph.facebook.com/debug_token?input_token={request.Token}&access_token={appAccessToken}";

            using HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(validationUrl);
            string result = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<FacebookTokenValidationResult>(result);
            var timeExpire = DateTimeOffset.FromUnixTimeSeconds(res.Data.expires_at);
            if (res.Data.is_valid && timeExpire.DateTime.AddHours(1) > DateTime.Now.ToUniversalTime())
            {
                string url = $"https://graph.facebook.com/v20.0/me?fields=id,name,email&access_token={request.Token}";

                HttpResponseMessage responseInfo = await client.GetAsync(url);
                responseInfo.EnsureSuccessStatusCode();

                string content = await responseInfo.Content.ReadAsStringAsync();
                var userProfile = JsonConvert.DeserializeObject<FacebookUser>(content);

                if (userProfile != null && !string.IsNullOrEmpty(userProfile.email))
                {
                    var checkUser = await _userRepository.FirstOrDefault(new FilterRequest().Filter("email", userProfile.email).Filter("type", EMPUserTypes.JobSeeker));
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
                        var passwordDefault = _configuration["PasswordDefault:password"];
                        
                        var user = new User()
                        {
                            UserName = userProfile.email,
                            FullName = userProfile.name,
                            IsActive = true,
                            IsSystem = false,
                            Email = userProfile.email,
                            AvatarUrl = null,
                            CreatedDate = DateTime.Now,
                            CreatedBy = Enum.GetName(typeof(OauthUserEnum), OauthUserEnum.Facebook)
                        };

                        await _userManager.CreateAsync(user, passwordDefault ?? "Buca@19006419");

                        // dùng thằng _userManager nên hiện tại phải làm kiểu này
                        var userDapper = MapHelper.Mapper<User, DapperUser>(user);
                        await _dapperUserRepository.UpdateAsync(userDapper);

                        var userInfo = _jwtTokenProvider.GetToken(user.GetUserInfo(), false);
                        _distributedCache.SetUserInfo($"Bearer {userInfo.Token}", userInfo);
                        return userInfo;
                    }
                }
                else
                {
                    throw new BusinessException("Không lấy được thông tin email, vui lòng kiểm tra lại", "Email error", request);
                }
            }
            else throw new BusinessException("Thông tin đăng nhập không chính xác, vui lòng kiểm tra lại, vui lòng kiểm tra lại", "Email error", request);
        }

        public class FacebookTokenValidationResult
        {
            public FacebookTokenData Data { get; set; }
        }

        public class FacebookTokenData
        {
            public bool is_valid { get; set; }
            public string user_id { get; set; }
            public string app_id { get; set; }
            public long expires_at { get; set; }
            public long issued_at { get; set; }
        }

        public class FacebookUser
        {
            public string id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
        }
    }
}
