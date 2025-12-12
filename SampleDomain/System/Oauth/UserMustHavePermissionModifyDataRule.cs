using Sample.Common.Exceptions;
using Sample.Common.UserSessions;
using System;

namespace Sample.Domain.System.Oauth
{
    public class UserMustHavePermissionModifyDataRule : IBusinessRule
    {
        private readonly IUserSession _userSession;
        private readonly long? _managementUnitId;

        public UserMustHavePermissionModifyDataRule(IUserSession userSession, long? managementUnitId)
        {
            _userSession = userSession;
            _managementUnitId = managementUnitId;
        }

        public string Message { get => "Bạn không có quyền chỉnh sửa, xóa dữ liệu này."; set { } }

        public bool IsBroken()
        {
            return !_userSession.IsLoggedIn
                || _userSession.UserInfo?.ManagementUnitID == null
                || _managementUnitId == null
                || _userSession.UserInfo?.ManagementUnitID != _managementUnitId;
        }
    }
}
