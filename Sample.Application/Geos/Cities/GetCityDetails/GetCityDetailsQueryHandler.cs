using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Domain.Geos.Cities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application.Geos.Cities.GetCityDetails
{
    public class GetCityDetailsQueryHandler : QueryHandlerBase<GetCityDetailsQuery, City>
    {
        private readonly IDapperRepository<City> _cityRepository;
        public GetCityDetailsQueryHandler(IDapperRepository<City> cityRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _cityRepository = cityRepository;
        }

        public override async Task<City> QueryHandle(GetCityDetailsQuery request, CancellationToken cancellationToken)
        {
            var res = await _cityRepository.GetAsync(request.CityCode);
            return res;
        }
    }
}
