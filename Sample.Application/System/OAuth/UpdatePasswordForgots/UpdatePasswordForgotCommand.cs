using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.OAuth.UpdatePasswordForgots
{
    public class UpdatePasswordForgotCommand : CommandBase<UserDto>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public UpdatePasswordForgotCommand(string userId, string password) 
        {
            UserId = userId;
            Password = password;
        }
    }
}
