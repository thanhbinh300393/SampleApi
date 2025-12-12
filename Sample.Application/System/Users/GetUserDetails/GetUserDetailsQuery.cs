using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Queries;

namespace Sample.Application.System.Users.GetUserDetails
{
    public class GetUserDetailsQuery : QueryBase<UserDto>
    {
        public string UserId { get; set; }
        public GetUserDetailsQuery(string userId)
        {
            this.UserId = userId;
        }

    }
}