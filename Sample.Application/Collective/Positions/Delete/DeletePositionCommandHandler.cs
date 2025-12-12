using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.Collective.Positions;
using Sample.Domain.Resources;

namespace Sample.Application.Collective.Positions.Delete
{
    public class DeletePositionCommandHandler : CommandHandlerBase<DeletePositionCommand, PositionDto>
    {
        private readonly IDapperRepository<Position> _positionRepository;
        public DeletePositionCommandHandler(
            IDapperRepository<Position> positionRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _positionRepository = positionRepository;
        }

        public override async Task<PositionDto> CommandHandle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _positionRepository.GetAsync(request.PositionID);
            if (entity == null)
                throw new NotFoundException(LanguageResource.DataNotFound, message: $"Position not found {request.PositionID}", data: request);
            if (entity.IsSystem == true)
                throw new BusinessException("Không thể xóa dữ liệu hệ thống", message: $"Position can not delete {request.PositionID}", null);
            try
            {
                await _positionRepository.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(LanguageResource.NoneBusinessArises, message: $"Position can not delete {request.PositionID}", ex);
            }

            if (entity != null)
                return MapHelper.Mapper<Position, PositionDto>(entity);
            return null;
        }
    }
}
