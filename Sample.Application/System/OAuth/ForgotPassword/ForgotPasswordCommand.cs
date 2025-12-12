using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Domain.System.Users.Dtos;

namespace Sample.Application.System.OAuth.ForgotPassword
{
    public class ForgotPasswordCommand : CommandBase<UserDto>
    {
        public UserInput Request { get; set; }
        public ForgotPasswordCommand(UserInput request) 
        {
            Request = request;
        }
    }
}
