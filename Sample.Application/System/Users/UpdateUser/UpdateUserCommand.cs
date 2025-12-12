using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.Users.UpdateUser
{
    public class UpdateUserCommand : CommandBase<UserDto>
    {
        public UpdateUserRequest Request { get; set; }
        public string UserName { get; set; }

        public UpdateUserCommand(UpdateUserRequest request, string userName)
        {
            Request = request;
            this.UserName = userName;
        }
    }
}
