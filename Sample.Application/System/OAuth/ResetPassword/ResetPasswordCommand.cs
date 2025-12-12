using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.OAuth.ResetPassword
{
    public class ResetPasswordCommand : CommandBase<UserDto>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public ResetPasswordCommand(string userId, string password)
        {
            UserId = userId;
            Password = password;
        }
    }
}
