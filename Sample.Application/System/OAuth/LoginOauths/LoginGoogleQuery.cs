using Sample.Common.CQRS.Queries;
using Sample.Common.UserSessions;

namespace Sample.Application.System.OAuth.LoginOauths
{
    public class LoginGoogleQuery : QueryBase<UserInfo>
    {
        public string GoogleToken { get; set; }
        public LoginGoogleQuery(string googleToken)
        {
            GoogleToken = googleToken;
        }
    }
}
