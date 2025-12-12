using Sample.Common.Database;
using System;

namespace Sample.Common.CQRS.Commands
{
    public class CommandBase : ICommand
    {
        public Guid RequestId { get; }
        public virtual DatabaseTypes? DatabaseType { get; protected set; } = DatabaseTypes.Default;

        public CommandBase()
        {
            this.RequestId = Guid.NewGuid();
        }

        protected CommandBase(Guid requestId)
        {
            this.RequestId = requestId;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid RequestId { get; }
        public virtual DatabaseTypes? DatabaseType { get; protected set; } = DatabaseTypes.Default;

        protected CommandBase()
        {
            this.RequestId = Guid.NewGuid();
        }

        protected CommandBase(Guid requestId)
        {
            this.RequestId = requestId;
        }
    }
}