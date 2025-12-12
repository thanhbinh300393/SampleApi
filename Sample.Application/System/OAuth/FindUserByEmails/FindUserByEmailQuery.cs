using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Queries;

namespace Sample.Application.System.OAuth.FindUserByEmails
{
    public class FindUserByEmailQuery : QueryBase<UserDto>
    {
        public string Email { get; set; }
        public FindUserByEmailQuery(string email)
        {
            Email = email;
        }
    }
}
