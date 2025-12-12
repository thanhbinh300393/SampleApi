using Sample.Common.Caching;
using System;

namespace Sample.Common.CQRS.Commands
{
    public abstract class InternalCommandBase : CommandBase, ICacheKey<InternalCommandBase>
    {
        public abstract string CacheKey { get; }

        protected InternalCommandBase(Guid id) : base(id)
        {
        }
    }
}