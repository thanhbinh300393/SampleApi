using Sample.Common.FilterList;
using Sample.Common.CQRS.Queries;
using Sample.Domain.Geos.Cities;

namespace Sample.Application.Geos.Cities.GetCities
{
    public class GetCitiesQuery : QueryBase<FilterResult<City>>
    {
        public FilterRequest DataRequest { get; set; }
        public GetCitiesQuery(FilterRequest dataRequest)
        {
            this.DataRequest = dataRequest;
        }
    }
}
