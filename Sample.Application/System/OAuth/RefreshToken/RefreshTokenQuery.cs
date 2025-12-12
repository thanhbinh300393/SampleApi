using Sample.Common.CQRS.Queries;
using Sample.Common.UserSessions;

namespace Sample.Application.System.Users.OAuth.RefreshToken
{
    public class RefreshTokenQuery : QueryBase<UserInfo>
    {
        public RefreshTokenQuery()
        {
        }
    }
}
