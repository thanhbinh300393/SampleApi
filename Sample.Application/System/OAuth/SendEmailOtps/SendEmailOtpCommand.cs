using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.OAuth.SendEmailOtps
{
    public class SendEmailOtpCommand : CommandBase
    {
        public string ToEmail { get; set; }
        public string UserId { get; set; }
        public SendEmailOtpCommand(string toEmail, string userId)
        {
            ToEmail = toEmail;
            UserId = userId;
        }
    }
}
