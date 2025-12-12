using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.FilterList;
using Sample.Common.Heplers;
using Sample.Domain.Resources;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.GetUserDetails
{
    public class GetUserDetailsQueryHandler : QueryHandlerBase<GetUserDetailsQuery, UserDto>
    {
        private readonly IDapperRepository<User> _userRepository;

        public GetUserDetailsQueryHandler(
            IDapperRepository<User> userRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserDto> QueryHandle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var user = (await _userRepository.GetAllAsync(
                            FilterRequest.CreateInstance()
                            .Filter("Id", userId)))?
                            .FirstOrDefault();
            if (user == null)
                throw new NotFoundException(LanguageResource.AccountNotFound, $"Account {request.UserId} not found", request);
            return MapHelper.Mapper<User, UserDto>(user);
        }
    }
}