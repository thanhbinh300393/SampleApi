using Sample.Application.System.Users.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.FilterList;

namespace Sample.Application.System.Users.Gets
{
    public class GetUsersQuery : QueryBase<FilterResult<UserDto>>
    {
        public FilterRequest DataRequest { get; set; }
        public GetUsersQuery(FilterRequest dataRequest)
        {
            this.DataRequest = dataRequest;
        }
    }
}
