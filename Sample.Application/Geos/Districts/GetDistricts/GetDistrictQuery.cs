using Sample.Common.FilterList;
using Sample.Common.CQRS.Queries;
using Sample.Domain.Geos.Districts;

namespace Sample.Application.Geos.Districts.GetDistricts
{
    public class GetDistrictQuery : QueryBase<FilterResult<District>>
    {
        public FilterRequest DataRequest { get; set; }
        public GetDistrictQuery(FilterRequest dataRequest)
        {
            this.DataRequest = dataRequest;
        }
    }
}
