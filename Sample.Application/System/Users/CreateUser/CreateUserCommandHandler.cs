using Microsoft.AspNetCore.Identity;
using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using User = Sample.Domain.System.Users.User;

namespace Sample.Application.System.Users.CreateUser
{
    public class CreateUserCommandHandler : CommandHandlerBase<CreateUserCommand, UserDto>
    {

        private readonly ISequenceProvider _sequenceProvider;
        private readonly UserManager<User> _userManager;

        public string passwordHash { get { return _configuration["PasswordDefault:password"] ?? ""; } }

        public CreateUserCommandHandler(
            ISequenceProvider sequenceProvider,
            UserManager<User> userManager,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _sequenceProvider = sequenceProvider;
            _userManager = userManager;
        }

        public override async Task<UserDto> CommandHandle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = User.Create(request.Request, _sequenceProvider, _userSession);
            if (string.IsNullOrEmpty(request.Request.Password))
            {
                var userExist = _userManager.FindByNameAsync(entity.UserName);
                while (userExist.Result != null)
                {
                    entity.UserName = entity.UserName + "001";
                    userExist = _userManager.FindByNameAsync(entity.UserName);
                }
                var rs = await _userManager.CreateAsync(entity, passwordHash);
                if (!rs.Succeeded)
                {
                    if (rs.Errors.Select(x => x.Code).Contains("DuplicateUserName"))
                    {
                        throw new BadRequestException("Tên đăng nhập đã tồn tại.");
                    }
                    throw new BadRequestException("Thêm mới người dùng không thành công.");
                }
            }
            else
            {
                var rs = await _userManager.CreateAsync(entity, request.Request.Password);
                if (!rs.Succeeded)
                {
                    if (rs.Errors.Select(x => x.Code).Contains("DuplicateUserName"))
                    {
                        throw new BadRequestException("Mã người dùng đã tồn tại.");
                    }
                    throw new BadRequestException("Thêm mới người dùng không thành công.");
                }
            }
            return MapHelper.Mapper<User, UserDto>(entity);
        }
    }
}
