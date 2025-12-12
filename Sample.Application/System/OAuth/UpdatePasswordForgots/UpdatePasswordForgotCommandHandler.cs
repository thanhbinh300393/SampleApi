using Microsoft.AspNetCore.Identity;
using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.System.Users;

namespace Sample.Application.System.OAuth.UpdatePasswordForgots
{
    public class UpdatePasswordForgotCommandHandler : CommandHandlerBase<UpdatePasswordForgotCommand, UserDto>
    {
        private readonly UserManager<User> _userManager;
        public UpdatePasswordForgotCommandHandler(UserManager<User> userManager, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userManager = userManager;
        }

        public override async Task<UserDto> CommandHandle(UpdatePasswordForgotCommand command, CancellationToken cancellationToken)
        {
            var userChangePass = await _userManager.FindByIdAsync(command.UserId) ?? throw new NotFoundException("Tài khoản không tồn tại", $"{command.UserId} not found", command);
            await _userManager.RemovePasswordAsync(userChangePass);
            await _userManager.AddPasswordAsync(userChangePass, command.Password);
            userChangePass.PasswordChangeDate = DateTime.Now;
            await _userManager.UpdateAsync(userChangePass);
            return MapHelper.Mapper<User, UserDto>(userChangePass);
        }
    }
}
