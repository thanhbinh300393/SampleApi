using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.Users.Delete
{
    public class DeleteUserCommand : CommandBase<UserDto?>
    {
        public string UserID { get; set; }
        public DeleteUserCommand(string userID)
        {
            UserID = userID;
        }
    }
}
