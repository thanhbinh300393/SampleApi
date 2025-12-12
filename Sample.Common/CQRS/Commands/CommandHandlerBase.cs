using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sample.Common.CQRS.DomainEvents;
using Sample.Common.CQRS.EventBus;
using Sample.Common.Database;
using Sample.Common.UserSessions;
using Serilog;

namespace Sample.Common.CQRS.Commands
{
    public abstract class CommandHandlerBase<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : CommandBase<TResult>
    {
        private readonly IServiceProvider _serviceProvider;
        private IEventBus _eventBus => (IEventBus)_serviceProvider.GetService(typeof(IEventBus));
        protected IMediator _mediator => (IMediator)_serviceProvider.GetService(typeof(IMediator));
        protected ILogger _logger => (ILogger)_serviceProvider.GetService(typeof(ILogger));
        protected IUserSession _userSession => (IUserSession)_serviceProvider.GetService(typeof(IUserSession));
        protected IConfiguration _configuration => (IConfiguration)_serviceProvider.GetService(typeof(IConfiguration));
        protected ISqlConnectionFactory _sqlConnectionFactory => (ISqlConnectionFactory)_serviceProvider.GetService(typeof(ISqlConnectionFactory));
        private IDapperUnitOfWork _unitOfWork => (IDapperUnitOfWork)_serviceProvider.GetService(typeof(IDapperUnitOfWork));

        protected List<IDomainEvent> _domainEvents;

        public CommandHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _domainEvents = new List<IDomainEvent>();
        }

        public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Debug($"{typeof(TCommand).Name}: {JsonConvert.SerializeObject(request)}");
                _unitOfWork.BeginTransaction(request.RequestId, request.DatabaseType);
                var result = await CommandHandle(request, cancellationToken);
                await PublishEvents(_domainEvents, cancellationToken);
                await _unitOfWork.Commit(request.RequestId);
                return result;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
            }
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

        public abstract Task<TResult> CommandHandle(TCommand command, CancellationToken cancellationToken);
    }

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand : CommandBase
    {
        private readonly IServiceProvider _serviceProvider;
        private IEventBus _eventBus => (IEventBus)_serviceProvider.GetService(typeof(IEventBus));
        protected IMediator _mediator => (IMediator)_serviceProvider.GetService(typeof(IMediator));
        protected ILogger _logger => (ILogger)_serviceProvider.GetService(typeof(ILogger));
        protected IUserSession _userSession => (IUserSession)_serviceProvider.GetService(typeof(IUserSession));
        protected IConfiguration _configuration => (IConfiguration)_serviceProvider.GetService(typeof(IConfiguration));
        protected ISqlConnectionFactory _sqlConnectionFactory => (ISqlConnectionFactory)_serviceProvider.GetService(typeof(ISqlConnectionFactory));
        private IDapperUnitOfWork _unitOfWork => (IDapperUnitOfWork)_serviceProvider.GetService(typeof(IDapperUnitOfWork));

        protected List<IDomainEvent> _domainEvents;

        public CommandHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _domainEvents = new List<IDomainEvent>();
        }

        public async Task Handle(TCommand request, CancellationToken cancellationToken = default)
        {
            try
            {
                _unitOfWork.BeginTransaction(request.RequestId, request.DatabaseType);

                await CommandHandle(request, cancellationToken);
                await PublishEvents(_domainEvents, cancellationToken);

                await _unitOfWork.Commit(request.RequestId);
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
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

        public abstract Task CommandHandle(TCommand command, CancellationToken cancellationToken);
    }
}
