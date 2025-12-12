using Microsoft.AspNetCore.Identity;
using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.Resources;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.UpdateUser
{
    public class UpdateUserCommandHandler : CommandHandlerBase<UpdateUserCommand, UserDto>
    {
        private readonly IDapperRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        public UpdateUserCommandHandler(
            IDapperRepository<User> userRepository,
            UserManager<User> userManager,

        IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public override async Task<UserDto> CommandHandle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByIdAsync(request.Request.Id);
            if (entity == null)
                throw new NotFoundException(LanguageResource.DataNotFound, $"User {request.Request.Id} not found", request);
            
            entity.Update(request.Request);
            var rs = await _userManager.UpdateAsync(entity);
            if (!rs.Succeeded)
            {
                throw new BadRequestException("Thêm mới người dùng không thành công.");
            }
            return MapHelper.Mapper<User, UserDto>(entity);
        }
    }
}
