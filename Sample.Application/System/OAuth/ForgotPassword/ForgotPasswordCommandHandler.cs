using Microsoft.AspNetCore.Identity;
using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.Resources;
using Sample.Domain.System.Users;

namespace Sample.Application.System.OAuth.ForgotPassword
{
    public class ForgotPasswordCommandHandler : CommandHandlerBase<ForgotPasswordCommand, UserDto>
    {
        private readonly IDapperRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;

        public ForgotPasswordCommandHandler(IDapperRepository<User> userRepository, UserManager<User> userManager, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public override async Task<UserDto> CommandHandle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Request.Email);

            if (user == null)
            {
                throw new NotFoundException(LanguageResource.DataNotFound, $"{request.Request.Email} not found", request);
            }
            else
            {
                await _userRepository.UpdateAsync(user);
                return MapHelper.Mapper<User, UserDto>(user);
            }
        }
    }
}
