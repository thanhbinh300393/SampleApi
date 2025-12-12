using System.Globalization;

namespace Sample.Common.UserSessions
{
    public interface IUserSession
    {
        UserInfo UserInfo { get; }

        void SetUserInfo(UserInfo userInfo);

        bool IsLoggedIn { get; }

        string Language { get; set; }

        CultureInfo CultureInfo { get; }
    }
}
