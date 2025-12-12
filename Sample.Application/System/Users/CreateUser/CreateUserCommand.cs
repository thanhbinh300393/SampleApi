using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.Users.CreateUser
{
    public class CreateUserCommand : CommandBase<UserDto>
    {
        public CreateUserRequest Request { get; set; }
        public CreateUserCommand(CreateUserRequest request)
        {
            Request = request;
        }
    }
}
