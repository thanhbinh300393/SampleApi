using Sample.Common.CQRS.Commands;
using Sample.Common.Heplers;

namespace Sample.Application.System.OAuth.VerifyOtps
{
    public class VerifyOtpCommandHandler : CommandHandlerBase<VerifyOtpCommand, bool>
    {
        public VerifyOtpCommandHandler(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }

        public override Task<bool> CommandHandle(VerifyOtpCommand command, CancellationToken cancellationToken)
        {
            var rs = OtpHelper.VerifyOTP(command.UserId, command.Otp);
            return Task.FromResult(rs);
        }
    }
}
