using Sample.Common.CQRS.EventBus;
using Sample.Common.UserSessions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Common.CQRS.DomainEvents
{
    public abstract class DomainEventHandlerBase<TEvent> : INotificationHandler<TEvent> where TEvent : DomainEventBase
    {
        private readonly IServiceProvider _serviceProvider;
        private IEventBus _eventBus => (IEventBus)_serviceProvider.GetService(typeof(IEventBus));
        protected IMediator _mediator => (IMediator)_serviceProvider.GetService(typeof(IMediator));
        protected ILogger _logger => (ILogger)_serviceProvider.GetService(typeof(ILogger));
        protected IUserSession _userSession => (IUserSession)_serviceProvider.GetService(typeof(IUserSession));
        protected IConfiguration _configuration => (IConfiguration)_serviceProvider.GetService(typeof(IConfiguration));

        protected List<IDomainEvent> _domainEvents;

        public DomainEventHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _domainEvents = new List<IDomainEvent>();
        }

        public async Task Handle(TEvent notification, CancellationToken cancellationToken = default)
        {
            await this.EventHandle(notification, cancellationToken);
            await PublishEvents(_domainEvents, cancellationToken);
        }

        private async Task PublishEvents(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
        {
            if (domainEvents == null) return;
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            var integrateEvents = domainEvents.Where(x => x.GetType().BaseType == typeof(IntegrationEventBase)).ToList();
            foreach (var integrateEvent in integrateEvents)
            {
                _ = _eventBus.Publish((IntegrationEventBase)integrateEvent);
            }
        }

        public abstract Task EventHandle(TEvent request, CancellationToken cancellationToken);
    }
}
