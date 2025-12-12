using System.Globalization;

namespace Sample.Common.UserSessions
{
    public class UserSession : IUserSession
    {
        private UserInfo _userInfo;

        public UserSession()
        {
        }

        public UserInfo UserInfo => _userInfo;

        public bool IsLoggedIn => _userInfo != null;

        public void SetUserInfo(UserInfo userInfo)
        {
            _userInfo = userInfo;
        }

        private string _language;

        public string Language
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_language))
                {
                    _language = "vi-VN";
                    _cultureInfo = new CultureInfo(_language);
                }
                return _language;
            }
            set
            {
                if (_language != value)
                    _cultureInfo = new CultureInfo(value);
                _language = value;
            }
        }

        private CultureInfo _cultureInfo;

        public CultureInfo CultureInfo
        {
            get
            {
                if (_cultureInfo == null)
                    _cultureInfo = new CultureInfo(Language);
                return _cultureInfo;
            }
        }

    }
}
