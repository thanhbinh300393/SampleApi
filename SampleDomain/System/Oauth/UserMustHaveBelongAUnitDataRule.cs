using Sample.Common.Exceptions;
using Sample.Common.UserSessions;
using Sample.Domain.Resources;

namespace Sample.Domain.System.Oauth
{
    public class UserMustHaveBelongAUnitDataRule : IBusinessRule
    {
        private readonly IUserSession _userSession;

        public UserMustHaveBelongAUnitDataRule(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public string Message { get => LanguageResource.AccountNotAssignManagementUnit; set { } }

        public bool IsBroken()
        {
            return !_userSession.IsLoggedIn
                || _userSession.UserInfo?.ManagementUnitID == null;
        }
    }
}
