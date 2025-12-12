using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.Collective.Positions.Update
{
    public class UpdatePositionCommand : CommandBase<PositionDto>
    {
        public UpdatePositionRequest Request { get; set; }
        public long PositionID { get; set; }
        public UpdatePositionCommand(UpdatePositionRequest request, long positionID)
        {
            PositionID = positionID;
            Request = request;
        }
    }
}
