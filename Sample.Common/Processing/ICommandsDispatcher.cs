using System;
using System.Threading.Tasks;

namespace Sample.Common.Processing
{
    public interface ICommandsDispatcher
    {
        Task DispatchCommandAsync(Guid id);
    }
}
