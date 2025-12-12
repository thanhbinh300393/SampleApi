using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Domain.Geos.Districts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application.Geos.Districts.GetDistricts
{
    public class GetDistrictQueryHandler : QueryHandlerBase<GetDistrictQuery, FilterResult<District>>
    {
        private readonly IDapperRepository<District> _districtRepository;
        public GetDistrictQueryHandler(IDapperRepository<District> districtRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _districtRepository = districtRepository;
        }

        public override async Task<FilterResult<District>> QueryHandle(GetDistrictQuery request, CancellationToken cancellationToken)
        {
            var res = await _districtRepository.Filter<District>(request.DataRequest);
            return res;
        }
    }
}
