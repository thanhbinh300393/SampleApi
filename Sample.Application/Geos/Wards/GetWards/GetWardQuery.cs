using Sample.Common.FilterList;
using Sample.Common.CQRS.Queries;
using Sample.Domain.Geos.Wards;

namespace Sample.Application.Geos.Wards.GetWards
{
    public class GetWardQuery : QueryBase<FilterResult<Ward>>
    {
        public FilterRequest DataRequest { get; set; }
        public GetWardQuery(FilterRequest dataRequest)
        {
            this.DataRequest = dataRequest;
        }
    }
}