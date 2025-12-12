using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Common.Heplers;
using Sample.Domain.System.Users;

namespace Sample.Application.System.OAuth.FindUserByEmails
{
    public class FindUserByEmailQueryHandler : QueryHandlerBase<FindUserByEmailQuery, UserDto>
    {
        private readonly IDapperRepository<User> _userRepository;
        public FindUserByEmailQueryHandler(IDapperRepository<User> userRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserDto> QueryHandle(FindUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefault(new FilterRequest().Filter("email", request.Email));
            if (user == null) {
                return null;
            }
            else
                return MapHelper.Mapper<User, UserDto>(user);
        }
    }
}
