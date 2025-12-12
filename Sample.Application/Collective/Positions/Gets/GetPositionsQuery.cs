using Sample.Common.FilterList;
using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Queries;

namespace Sample.Application.Collective.Positions.Gets
{
    public class GetPositionsQuery : QueryBase<FilterResult<PositionDto>>
    {
        public FilterRequest DataRequest { get; set; }
        public GetPositionsQuery (FilterRequest dataRequest)
        {
            this.DataRequest = dataRequest;
        }
    }
}
