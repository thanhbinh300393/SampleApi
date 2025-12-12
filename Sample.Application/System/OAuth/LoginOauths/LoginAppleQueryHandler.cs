using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Sample.Application.System.OAuth.LoginOauths;

public class LoginAppleQueryHandler : QueryHandlerBase<LoginAppleQuery, UserInfo>
{
    private readonly IDapperRepository<User> _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly IDistributedCache _distributedCache;
    private readonly IDapperRepository<DapperUser> _dapperUserRepository;


    public LoginAppleQueryHandler(IServiceProvider serviceProvider, IDapperRepository<User> userRepository, UserManager<User> userManager, IJwtTokenProvider jwtTokenProvider, IDistributedCache distributedCache, IDapperRepository<DapperUser> dapperUserRepository) : base(serviceProvider)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtTokenProvider = jwtTokenProvider;
        _distributedCache = distributedCache;
        _dapperUserRepository = dapperUserRepository;
    }

    public override async Task<UserInfo> QueryHandle(LoginAppleQuery request, CancellationToken cancellationToken)
    {
        var appleKeys = await LoginAppleQueryExtensions.GetApplePublicKeysAsync();
        var signingKeys = appleKeys.Keys.Select(LoginAppleQueryExtensions.GenerateSecurityKeyFromAppleKey);

        var handler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = signingKeys,
            ValidateIssuer = true,
            ValidIssuer = "https://appleid.apple.com",
            ValidateAudience = true,
            ValidAudience = _configuration["EmploymentServiceCenterInfo:AppId"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        ClaimsPrincipal principal = handler.ValidateToken(
            request.AppleToken,
            validationParameters,
            out SecurityToken validatedToken);

        // Nếu thành công, bạn có thể lấy "sub" (Apple userId), email
        var jwtToken = (JwtSecurityToken)validatedToken;
        var appleUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var checkUser = await _userRepository.FirstOrDefault(new FilterRequest().Filter("email", email).Filter("type", EMPUserTypes.JobSeeker));
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
                UserName = email,
                FullName = email,
                IsActive = true,
                IsSystem = false,
                Email = email,
                CreatedDate = DateTime.Now,
                CreatedBy = Enum.GetName(typeof(OauthUserEnum), OauthUserEnum.Google)
            };

            await _userManager.CreateAsync(user, passwordDefault ?? "Buca@19006419");

            var userDapper = MapHelper.Mapper<User, DapperUser>(user);
            await _dapperUserRepository.UpdateAsync(userDapper);

            var userInfo = _jwtTokenProvider.GetToken(user.GetUserInfo(), false);
            _distributedCache.SetUserInfo($"Bearer {userInfo.Token}", userInfo);
            return userInfo;
        }
    }
}

public class ApplePublicKey
{
    public List<Key> Keys { get; set; } = new();

    public class Key
    {
        public string kty { get; set; } = string.Empty;
        public string kid { get; set; } = string.Empty;
        public string use { get; set; } = string.Empty;
        public string alg { get; set; } = string.Empty;
        public string n { get; set; } = string.Empty;
        public string e { get; set; } = string.Empty;
    }
}

public static class LoginAppleQueryExtensions
{
    public static SecurityKey GenerateSecurityKeyFromAppleKey(ApplePublicKey.Key key)
    {
        // Chuyển key n, e từ base64url -> byte[]
        var e = Base64UrlDecode(key.e);
        var n = Base64UrlDecode(key.n);

        var rsa = new RSAParameters
        {
            Exponent = e,
            Modulus = n
        };

        return new RsaSecurityKey(rsa)
        {
            KeyId = key.kid
        };
    }

    // Lấy và cache public keys từ Apple
    public static async Task<ApplePublicKey> GetApplePublicKeysAsync()
    {
        using var httpClient = new HttpClient();
        var json = await httpClient.GetStringAsync("https://appleid.apple.com/auth/keys");
        return JsonConvert.DeserializeObject<ApplePublicKey>(json)!;
    }

    // Base64UrlDecode (loại bỏ ký tự '-','_' so với chuẩn base64)
    private static byte[] Base64UrlDecode(string base64Url)
    {
        string padded = base64Url;
        padded = padded.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
        }

        return Convert.FromBase64String(padded);
    }
}