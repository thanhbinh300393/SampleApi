using Microsoft.AspNetCore.Identity;
using Sample.Common.CQRS.Commands;
using Sample.Common.Exceptions;
using Sample.Common.UserSessions;
using Sample.Domain.Resources;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.ChangePassword
{
    public class ChangePasswordCommandHandler : CommandHandlerBase<ChangePasswordCommand, UserInfo>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(
            UserManager<User> userManager,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userManager = userManager;
        }

        public override async Task<UserInfo> CommandHandle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userInfor = _userSession.UserInfo;
            var user = await _userManager.FindByIdAsync(userInfor.UserId);
            if (user == null)
                throw new NotFoundException(LanguageResource.AccountNotFound, $"Account {userInfor.UserId} not found", request);
            var checkPass = await _userManager.CheckPasswordAsync(user, request.Data.CurrentPassword);
            if (!checkPass)
                throw new BadRequestException("Mật khẩu hiện tại không chính xác");
            var rs = await _userManager.ChangePasswordAsync(user, request.Data.CurrentPassword, request.Data.NewPassword);
            if (!rs.Succeeded)
                throw new BadRequestException(LanguageResource.InputDataIsWrong);
            user.PasswordChangeDate = DateTime.Now;
            await _userManager.UpdateAsync(user);
            return userInfor;
        }
    }
}
