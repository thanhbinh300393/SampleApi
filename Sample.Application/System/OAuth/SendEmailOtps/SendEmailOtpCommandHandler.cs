using Microsoft.AspNetCore.Identity;
using Sample.Application.SendMails.SendMailsByEa;
using Sample.Common.CQRS.Commands;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.System.Users;

namespace Sample.Application.System.OAuth.SendEmailOtps
{
    public class SendEmailOtpCommandHandler : CommandHandlerBase<SendEmailOtpCommand>
    {
        private readonly UserManager<User> _userManager;
        public SendEmailOtpCommandHandler(UserManager<User> userManager, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userManager = userManager;
        }

        public override async Task CommandHandle(SendEmailOtpCommand command, CancellationToken cancellationToken)
        {
            var userChangePass = await _userManager.FindByIdAsync(command.UserId) ?? throw new NotFoundException("Tài khoản không tồn tại", $"{command.UserId} not found", command);
            var otp = OtpHelper.GenerateOTP(command.UserId);

            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot\\Templates\\EmailBody.txt");
            string emailBody = File.ReadAllText(templatePath);
            emailBody = emailBody.Replace("{{FullName}}", userChangePass?.FullName ?? "");
            emailBody = emailBody.Replace("{{Action}}", "Bạn vừa yêu cầu đặt lại mật khẩu cho tài khoản của mình");
            emailBody = emailBody.Replace("{{Title}}", "Mã xác thực đặt lại mật khẩu");
            emailBody = emailBody.Replace("{{Otp}}", otp.ToString());

            await _mediator.Send(new SendMailsByEaCommand(new SendMailsByEaRequest
            {
                ToEmail = command.ToEmail,
                Subject = otp + " là mã khôi phục tài khoản của bạn",
                TextBody = emailBody
            }), cancellationToken);
        }
    }
}
