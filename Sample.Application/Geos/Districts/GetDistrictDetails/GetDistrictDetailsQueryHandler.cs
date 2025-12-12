using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Domain.Geos.Districts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application.Geos.Districts.GetDistrictDetails
{
    public class GetDistrictDetailsQueryHandler : QueryHandlerBase<GetDistrictDetailsQuery, District>
    {
        private readonly IDapperRepository<District> _districtRepository;
        public GetDistrictDetailsQueryHandler(IDapperRepository<District> districtRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _districtRepository = districtRepository;
        }

        public override async Task<District> QueryHandle(GetDistrictDetailsQuery request, CancellationToken cancellationToken)
        {
            var res = await _districtRepository.GetAsync(request.DistrictCode);
            return res;
        }
    }
}
