using Sample.Common.Dependency;

namespace Sample.Domain.Collective.Positions
{
    public interface IPositionChecker : ITransientDependency
    {
        bool IsExistCode(string code, long? positionID);
    }
}
