using Sample.Common.CQRS.Commands;

namespace Sample.Application.System.OAuth.VerifyOtps
{
    public class VerifyOtpCommand : CommandBase<bool>
    {
        public string Otp {  get; set; }
        public string UserId { get; set; }
        public VerifyOtpCommand(string otp, string userId)
        {
            Otp = otp;
            UserId = userId;
        }
    }
}
