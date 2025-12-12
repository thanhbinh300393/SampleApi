
using Sample.Common.Dependency;

namespace Sample.Common.Application
{
    public interface IApplicationServiceBase : ITransientDependency
    {
    }

    public abstract class ApplicationServiceBase : IApplicationServiceBase
    {
    }
}
