using Sample.Application.System.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.Resources;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.Delete
{
    public class DeleteUserCommandHandler : CommandHandlerBase<DeleteUserCommand, UserDto?>
    {
        private readonly ISequenceProvider _sequenceProvider;
        private readonly UserManager<User> _userManager;
        public DeleteUserCommandHandler(
            ISequenceProvider sequenceProvider,
            UserManager<User> userManager,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _sequenceProvider = sequenceProvider;
            _userManager = userManager;
        }

        public override async Task<UserDto?> CommandHandle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByIdAsync(request.UserID);
            if (entity == null)
                throw new NotFoundException(LanguageResource.DataNotFound, message: $"User not found {request.UserID}", data: request);
            try
            {
                await _userManager.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(LanguageResource.NoneBusinessArises, message: $"User can not delete {request.UserID}", ex);
            }

            if (entity != null)
                return MapHelper.Mapper<User, UserDto>(entity);
            return null;
        }
    }
}
