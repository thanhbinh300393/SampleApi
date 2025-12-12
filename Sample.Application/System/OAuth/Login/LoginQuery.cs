using Sample.Common.CQRS.Queries;
using Sample.Common.UserSessions;

namespace Sample.Application.System.Users.OAuth.Login
{
    public class LoginQuery : QueryBase<UserInfo>
    {
        public LoginRequest Data { get; set; }

        public LoginQuery(LoginRequest data)
        {
            Data = data;
        }
    }
}
