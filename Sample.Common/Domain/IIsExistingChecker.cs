using Sample.Common.Dependency;

namespace Sample.Common.Domain
{
    public interface IIsExistingChecker : ITransientDependency
    {
        bool IsExistByCode(string code, object idExclude = null);
        bool IsExistById(object id);
    }
}
