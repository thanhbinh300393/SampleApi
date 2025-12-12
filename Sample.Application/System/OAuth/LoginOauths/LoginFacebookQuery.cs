using Sample.Common.CQRS.Queries;
using Sample.Common.UserSessions;

namespace Sample.Application.System.OAuth.LoginOauths
{
    public class LoginFacebookQuery : QueryBase<UserInfo>
    {
        public string Token { get; set; }

        public LoginFacebookQuery(string token)
        {
            Token = token;
        }
    }
}
