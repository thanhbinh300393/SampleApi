using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.OAuth.SignUpUsers
{
    public class SignUpUserCommand : CommandBase<SignUpUserDto>
    {
        public SignUpUserRequest SignUpUserRequest { get; set; }
        public SignUpUserCommand(SignUpUserRequest signUpUserRequest)
        {
            SignUpUserRequest = signUpUserRequest;
        }
    }
}
