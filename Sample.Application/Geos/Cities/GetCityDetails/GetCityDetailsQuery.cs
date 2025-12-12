using Sample.Common.CQRS.Queries;
using Sample.Domain.Geos.Cities;

namespace Sample.Application.Geos.Cities.GetCityDetails
{
    public class GetCityDetailsQuery : QueryBase<City>
    {
        public string CityCode { get; set; }

        public GetCityDetailsQuery(string cityCode)
        {
            CityCode = cityCode;
        }
    }
}
