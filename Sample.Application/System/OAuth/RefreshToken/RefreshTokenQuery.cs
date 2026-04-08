using Sample.Common.CQRS.Queries;
using Sample.Common.UserSessions;

namespace Sample.Application.System.Users.OAuth.RefreshToken
{
    public class RefreshTokenQuery : QueryBase<UserInfo>
    {
        public string RefreshToken { get; set; }
        public RefreshTokenQuery(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
