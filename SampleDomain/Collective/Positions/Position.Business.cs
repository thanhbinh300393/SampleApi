using Sample.Common.Heplers;
using Sample.Common.UserSessions;
using Sample.Common.Extentions;
using Sample.Domain.Collective.Positions.Dto;
using Sample.Domain.Collective.Positions.Rules;
using Sample.Common.Domain;

namespace Sample.Domain.Collective.Positions
{
    public partial class Position
    {
        public static Position Create(
           PositionInput input,
           ISequenceProvider sequenceProvider,
           IUserSession userSession,
           IPositionChecker checker)
        {
            var entity = MapHelper.Mapper<PositionInput, Position>(input);
            CheckRule(new PositionCodeMustBeUniqueRule(checker, input.Code, null));
            entity.PositionID = sequenceProvider.GetLongValue<Position>().Await();
            //entity.ManagementUnitID = userSession.UserInfo.ManagementUnitID;
            //entity.CreatedBy = userSession.UserInfo.UserId;
            return entity;
        }

        public void Update(PositionInput input, IUserSession userSession)
        {
            // CheckRule(new PositionCodeMustBeUniqueRule(checker, input.Code, userSession.UserInfo.ManagementUnitID, input.ModuleID, this.PositionID));
            var entity = this;
            entity.SetAuditUpdation(userSession);
            MapHelper.Mapper(input, ref entity);
        }
    }
}
