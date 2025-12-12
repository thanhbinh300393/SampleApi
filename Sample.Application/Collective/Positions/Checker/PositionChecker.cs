using Sample.Common.Domain;
using Sample.Common.Extentions;
using Sample.Common.FilterList;
using Sample.Domain.Collective.Positions;
using Sample.Domain.Configuration;

namespace Sample.Application.Collective.Positions.Checker
{
    public class PositionChecker : IPositionChecker, IChecker
    {
        public IDapperRepository<Position> _positionRepository;
        public PositionChecker(IDapperRepository<Position> positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public bool IsExistCode(string code, long? positionID)
        {

            return _positionRepository.GetAllAsync(FilterRequest.CreateInstance()
                                    .Filter("code", code)
                                    ).Await().WhereIf(positionID.HasValue, x => x.PositionID != positionID).Any();
        }
    }
}
