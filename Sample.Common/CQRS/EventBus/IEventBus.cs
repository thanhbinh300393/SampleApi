using Sample.Common.CQRS.DomainEvents;
using Sample.Common.Dependency;
using System.Threading.Tasks;

namespace Sample.Common.CQRS.EventBus
{
    public interface IEventBus : ISingletonDependency
    {
        Task Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IntegrationEventBase;
        Task Execute(string eventName, string dataJson);
    }
}
