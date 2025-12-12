using Sample.Common.Dependency;
using Sample.Common.UserSessions;

namespace Sample.Domain.System.Oauth
{
    public interface IJwtTokenProvider : ISingletonDependency
    {
        UserInfo GetToken(UserInfo user, bool isRemember);
        UserInfo GetInfoByToken(string token);
    }
}
