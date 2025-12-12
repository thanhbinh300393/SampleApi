using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.Collective.Positions.Delete
{
    public class DeletePositionCommand : CommandBase<PositionDto>
    {
        public long PositionID { get; set; }
        public DeletePositionCommand(long positionID)
        {
            PositionID = positionID;
        }
    }
}
