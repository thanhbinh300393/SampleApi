using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Domain.Geos.Wards;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Application.Geos.Wards.GetWardDetails
{
    public class GetWardDetailsQueryHandler : QueryHandlerBase<GetWardDetailsQuery, Ward>
    {
        private readonly IDapperRepository<Ward> _wardRepository;
        public GetWardDetailsQueryHandler(IDapperRepository<Ward> wardRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _wardRepository = wardRepository;
        }

        public override async Task<Ward> QueryHandle(GetWardDetailsQuery request, CancellationToken cancellationToken)
        {
            var res = await _wardRepository.GetAsync(request.WardCode);
            return res;
        }
    }
}