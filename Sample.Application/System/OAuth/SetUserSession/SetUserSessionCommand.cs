using Sample.Common.CQRS.Commands;
using Sample.Common.UserSessions;

namespace Sample.Application.System.OAuth.SetUserSession
{
    public class SetUserSessionCommand : CommandBase<UserInfo>
    {
        public long ModuleID { get; set; }

        public SetUserSessionCommand(long moduleID)
        {
            ModuleID = moduleID;
        }
    }
}
