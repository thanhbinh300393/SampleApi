using Sample.Application.Collective.Positions.Dto;
using Sample.Common.CQRS.Commands;

namespace Sample.Application.Collective.Positions.GetDetail
{
    public class GetPositionDetailQuery : CommandBase<PositionDto>
    {
        public long PositionID { get; set; }
        public GetPositionDetailQuery(long positionID)
        {
            PositionID = positionID;
        }
    }
}
