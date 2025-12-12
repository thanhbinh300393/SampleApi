using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Heplers;
using Sample.Domain.Collective.Positions;

namespace Sample.Application.Collective.Positions.Create
{
    class CreatePositionCommandHandler : CommandHandlerBase<CreatePositionCommand, PositionDto>
    {
        private readonly IDapperRepository<Position> _positionRepository;
        private readonly IPositionChecker _checker;
        private readonly ISequenceProvider _sequenceProvider;

        public CreatePositionCommandHandler(
            IDapperRepository<Position> positionRepository,
            ISequenceProvider sequenceProvider,
            IPositionChecker checker,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _positionRepository = positionRepository;
            _checker = checker;
            _sequenceProvider = sequenceProvider;
        }

        public override async Task<PositionDto> CommandHandle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = Position.Create(request.Request, _sequenceProvider, _userSession, _checker);
            await _positionRepository.InsertAsync(entity);
            return MapHelper.Mapper<Position, PositionDto>(entity);
        }
    }
}
