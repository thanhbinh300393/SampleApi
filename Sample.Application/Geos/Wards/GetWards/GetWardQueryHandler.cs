using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Domain.Geos.Wards;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application.Geos.Wards.GetWards
{
    public class GetWardQueryHandler : QueryHandlerBase<GetWardQuery, FilterResult<Ward>>
    {
        private readonly IDapperRepository<Ward> _wardRepository;
        public GetWardQueryHandler(IDapperRepository<Ward> wardRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _wardRepository = wardRepository;
        }

        public override async Task<FilterResult<Ward>> QueryHandle(GetWardQuery request, CancellationToken cancellationToken)
        {
            var res = await _wardRepository.Filter<Ward>(request.DataRequest);
            return res;
        }
    }
}