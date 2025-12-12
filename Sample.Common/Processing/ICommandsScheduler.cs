using Sample.Common.CQRS.Commands;
using System.Threading.Tasks;

namespace Sample.Common.Processing
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(InternalCommandBase command);
        Task EnqueueOutBoxAsync(InternalCommandBase command);
        Task EnqueueOutBoxToDbAsync();
    }
}