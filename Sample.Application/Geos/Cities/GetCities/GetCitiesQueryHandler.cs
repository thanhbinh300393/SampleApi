using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Domain.Geos.Cities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application.Geos.Cities.GetCities
{
    public class GetCitiesQueryHandler : QueryHandlerBase<GetCitiesQuery, FilterResult<City>>
    {
        private readonly IDapperRepository<City> _cityRepository;
        public GetCitiesQueryHandler(IDapperRepository<City> cityRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _cityRepository = cityRepository;
        }

        public override async Task<FilterResult<City>> QueryHandle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var res = await _cityRepository.Filter<City>(request.DataRequest);
            return res;
        }
    }
}
