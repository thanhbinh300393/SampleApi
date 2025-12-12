using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Domain.System.Users;

namespace Sample.Application.System.Users.Gets
{
    public class GetUsersQueryHandler : QueryHandlerBase<GetUsersQuery, FilterResult<UserDto>>
    {
        private readonly IDapperRepository<User> _userRepository;
        public GetUsersQueryHandler(
            IDapperRepository<User> userRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
        }

        public override async Task<FilterResult<UserDto>> QueryHandle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.Filter<UserDto>(request.DataRequest);
           
            return result;
        }
    }
}
