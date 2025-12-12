using System;

namespace Sample.Common.CQRS.DomainEvents
{
    public abstract class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            RequestId = Guid.NewGuid();
            this.OccurredOn = DateTime.Now;
        }

        public Guid RequestId { get; set; }
        public DateTime OccurredOn { get; }
    }
}