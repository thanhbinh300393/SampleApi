using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;
using Sample.Common.Domain;
using Sample.Common.Exceptions;
using Sample.Common.Heplers;
using Sample.Domain.Collective.Positions;
using Sample.Domain.Resources;

namespace Sample.Application.Collective.Positions.GetDetail
{
    public class GetPositionDetailQueryHandler : CommandHandlerBase<GetPositionDetailQuery, PositionDto>
    {
        public readonly IDapperRepository<Position> _positionRepository;
        public GetPositionDetailQueryHandler(
          IDapperRepository<Position> positionRepository,
          IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _positionRepository = positionRepository;
        }
        public override async Task<PositionDto> CommandHandle(GetPositionDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _positionRepository.GetAsync(request.PositionID);
            if (entity == null)
                throw new BadRequestException(LanguageResource.DataNotFound, "Không tìm thấy chức vụ", data: request);
            return MapHelper.Mapper<Position, PositionDto>(entity); ;
        }
    }
}
