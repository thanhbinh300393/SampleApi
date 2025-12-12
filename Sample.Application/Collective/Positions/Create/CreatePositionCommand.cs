using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.Collective.Positions.Create
{
    public class CreatePositionCommand : CommandBase<PositionDto>
    {
        public CreatePositionRequest Request { get; set; }
        public CreatePositionCommand(CreatePositionRequest request)
        {
            Request = request;
        }
    }
}
