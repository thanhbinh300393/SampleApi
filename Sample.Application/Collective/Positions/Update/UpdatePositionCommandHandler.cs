using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.Collective.Positions;
using Sample.Domain.Resources;

namespace Sample.Application.Collective.Positions.Update
{
    class UpdatePositionCommandHandler : CommandHandlerBase<UpdatePositionCommand, PositionDto>
    {
        private readonly IDapperRepository<Position> _positionRepository;
        //private readonly IPositionChecker _positionsChecker;
        public UpdatePositionCommandHandler(
            IDapperRepository<Position> positionRepository,
            //IPositionChecker positionsChecker,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _positionRepository = positionRepository;
            //_positionsChecker = positionsChecker;
        }

        public override async Task<PositionDto> CommandHandle(UpdatePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _positionRepository.GetAsync(request.PositionID);
            if (entity == null)
                throw new NotFoundException(LanguageResource.DataNotFound, $"Position {request.PositionID} not found", request);
            entity.Update(request.Request, _userSession);
            await _positionRepository.UpdateAsync(entity);
            return MapHelper.Mapper<Position, PositionDto>(entity);
        }
    }
}
