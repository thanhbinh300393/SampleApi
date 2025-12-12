using Sample.Common.UserSessions;
using Serilog;
using System;
using System.Globalization;
using System.Resources;

namespace Sample.Common.Languages
{
    public interface ILanguageProvider<T> where T : class
    {
        string this[string key, object obj1 = null, object obj2 = null, object obj3 = null] { get; }
        string GetText(string key, object obj1 = null, object obj2 = null, object obj3 = null);
    }

    public class LanguageProvider<T> : ILanguageProvider<T> where T : class
    {
        private readonly IUserSession _userSession;
        private readonly ResourceManager _resourceManager;
        private readonly ILogger _logger;
        private CultureInfo _cultureInfo;

        public LanguageProvider(IUserSession userSession, ILogger logger)
        {
            _userSession = userSession;
            if (_userSession == null)
                _cultureInfo = new CultureInfo(LanguageConstants.LanguageDefault);
            _resourceManager = new ResourceManager(typeof(T));
            _logger = logger;
        }

        public void SetLanguage(string langCode)
        {
            _cultureInfo = new CultureInfo(langCode);
        }

        public CultureInfo GetCultureInfo()
        {
            if (_userSession != null)
                return _userSession.CultureInfo;
            else return _cultureInfo ?? new CultureInfo(LanguageConstants.LanguageDefault);
        }

        public string this[string key, object obj1 = null, object obj2 = null, object obj3 = null] => GetText(key, obj1, obj2, obj3);

        public string GetText(string key, object obj1 = null, object obj2 = null, object obj3 = null)
        {
            try
            {
                key = key ?? String.Empty;
                string str = key.Contains(" ") ? key : _resourceManager.GetString(key, _userSession.CultureInfo);
                return string.Format(str, obj1, obj2, obj3);
            }
            catch (Exception)
            {
                return key;
            }
        }
    }
}
