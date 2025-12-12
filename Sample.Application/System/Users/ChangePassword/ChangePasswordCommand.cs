using Sample.Common.CQRS.Commands;
using Sample.Common.UserSessions;

namespace Sample.Application.System.Users.ChangePassword
{
    public class ChangePasswordCommand : CommandBase<UserInfo>
    {
        public ChangePasswordRequest Data { get; set; }

        public ChangePasswordCommand(ChangePasswordRequest data)
        {
            Data = data;
        }
    }
}
