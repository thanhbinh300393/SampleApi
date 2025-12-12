using MediatR;
using System;

namespace Sample.Common.CQRS.DomainEvents
{
    public interface IDomainEvent : INotification
    {
        public Guid RequestId { get; }
        DateTime OccurredOn { get; }
    }
}