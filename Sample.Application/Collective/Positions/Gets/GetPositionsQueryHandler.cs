using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Queries;
using Sample.Common.Domain;
using Sample.Common.FilterList;
using Sample.Domain.Collective.Positions;

namespace Sample.Application.Collective.Positions.Gets
{
    public class GetPositionsQueryHandler : QueryHandlerBase<GetPositionsQuery, FilterResult<PositionDto>>
    {
        private readonly IDapperRepository<Position> _positionsRepository;

        public GetPositionsQueryHandler(
            IDapperRepository<Position> positionsRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _positionsRepository = positionsRepository;
        }

        public override async Task<FilterResult<PositionDto>> QueryHandle(GetPositionsQuery request, CancellationToken cancellationToken)
        {
            return await _positionsRepository.Filter<PositionDto>(request.DataRequest);
        }
    }
}
