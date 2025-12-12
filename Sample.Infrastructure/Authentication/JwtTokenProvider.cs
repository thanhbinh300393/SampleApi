using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sample.Common.Extentions;
using Sample.Common.UserSessions;
using Sample.Domain.System.Oauth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sample.Infrastructure.Authentication
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly IConfiguration _configuration;
        public string SecretKey { get { return this._configuration[JwtTokenConstants.SecretKey] ?? ""; } }
        public string Issuer { get { return this._configuration[JwtTokenConstants.Issuer] ?? ""; } }
        public string Audience { get { return this._configuration[JwtTokenConstants.Audience] ?? ""; } }

        public JwtTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserInfo GetToken(UserInfo user, bool isRemember)
        {
            var Expires = isRemember ? DateTime.Now.Date.AddDays(8).AddMilliseconds(-1) : DateTime.Now.AddMinutes(240).AddMilliseconds(-1);
            user.ExpiryDate = Expires;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = GetClaims(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Issuer,
                Audience = Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = Expires,
                SigningCredentials = credentials
            };

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Issuer,
                Audience = Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Date.AddDays(60),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.RefreshToken = tokenHandler.WriteToken(refreshToken);
            return user;
        }

        public UserInfo GetInfoByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidAudience = Audience,
                IssuerSigningKey = credentials.Key,
                ClockSkew = TimeSpan.Zero,
                SaveSigninToken = true,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return GetInfoByClaims(jwtToken.Claims);
        }

        public static UserInfo GetInfoByClaims(IEnumerable<Claim> claims)
        {
            return new UserInfo()
            {
                UserId = GetClaimValue(claims, ClaimTypes.NameIdentifier) ?? GetClaimValue(claims, "nameid"),
                UserName = GetClaimValue(claims, ClaimTypes.Name) ?? GetClaimValue(claims, "unique_name"),
                Email = GetClaimValue(claims, ClaimTypes.Email) ?? GetClaimValue(claims, "email"),
                PhoneNumber = GetClaimValue(claims, ClaimTypes.MobilePhone) ?? GetClaimValue(claims, "phone_number"),
                FullName = GetClaimValue(claims, "FullName"),
                IsActive = GetClaimValue(claims, "IsActive").ToBool(),
                Type = GetClaimValue(claims, "Type").ToInt()
            };
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string key)
        {
            return claims.FirstOrDefault(x => x.Type == key)?.Value;
        }

        public static List<Claim> GetClaims(UserInfo user)
        {
            return new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                    new Claim("FullName", user.FullName ?? ""),
                    new Claim("IsActive", user.IsActive.ToString() ?? ""),
                    new Claim("Type", user.Type.ToString() ?? "")
                };
        }
    }
}
