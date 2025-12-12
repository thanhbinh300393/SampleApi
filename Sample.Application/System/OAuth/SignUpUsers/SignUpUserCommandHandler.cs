using Microsoft.AspNetCore.Identity;
using Sample.Application.SendMails.SendMailsByEa;
using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.FilterList;
using Sample.Common.Heplers;
using Sample.Domain.System.Users;

namespace Sample.Application.System.OAuth.SignUpUsers
{
    public class SignUpUserCommandHandler : CommandHandlerBase<SignUpUserCommand, SignUpUserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IDapperRepository<DapperUser> _dapperUserRepository;
        private readonly ISequenceProvider _sequenceProvider;
        public SignUpUserCommandHandler(UserManager<User> userManager,
            ISequenceProvider sequenceProvider,
            IDapperRepository<DapperUser> dapperUserRepository,
            IServiceProvider _serviceProvider) : base(_serviceProvider)
        {
            _userManager = userManager;
            _dapperUserRepository = dapperUserRepository;
            _sequenceProvider = sequenceProvider;
        }

        public override async Task<SignUpUserDto> CommandHandle(SignUpUserCommand command, CancellationToken cancellationToken)
        {

            if (command.SignUpUserRequest.Type == (int)EMPUserTypes.JobSeeker)
            {
                var userEmail = await _dapperUserRepository.FirstOrDefault(new FilterRequest().Filter("email", command.SignUpUserRequest.UserName));
                if (userEmail != null)
                    throw new BusinessException("Email đã được sử dụng, vui lòng kiểm tra lại", "Email " + command.SignUpUserRequest.UserName + " is used", command);
            }

            var user = new User()
            {
                FullName = command.SignUpUserRequest.FullName,
                IsActive = true,
                IsSystem = false,
                Email = command.SignUpUserRequest.UserName,
                UserName = command.SignUpUserRequest.UserName,
                CreatedDate = DateTime.Now,
                CreatedBy = "MOBILE_APP"
            };

            var otp = OtpHelper.GenerateOTP(user.Id);

            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot\\Templates\\EmailBody.txt");
            string emailBody = File.ReadAllText(templatePath);
            emailBody = emailBody.Replace("{{FullName}}", user?.FullName ?? "");
            emailBody = emailBody.Replace("{{Action}}", "Bạn vừa yêu cầu tạo mới tài khoản");
            emailBody = emailBody.Replace("{{Title}}", "Mã xác thực tạo tài khoản");
            emailBody = emailBody.Replace("{{Otp}}", otp.ToString());

            await _mediator.Send(new SendMailsByEaCommand(new SendMailsByEaRequest
            {
                ToEmail = command.SignUpUserRequest.UserName,
                Subject = otp + " là mã xác thực tài khoản của bạn",
                TextBody = emailBody
            }), cancellationToken);

            var userJobSeeker = MapHelper.Mapper<User, SignUpUserDto>(user);
            return userJobSeeker;
        }
    }
}
