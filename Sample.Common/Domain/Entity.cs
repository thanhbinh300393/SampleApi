using Sample.Common.CQRS.DomainEvents;
using Sample.Common.Exceptions;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sample.Common.Domain
{
    public interface IEntity : ISqlModel
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }

    /// <summary>
    /// Base class for entities.
    /// </summary>
    public abstract class Entity : IEntity
    {
        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        [NotMapped]
        [Computed]
        [JsonIgnore]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            this._domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clear domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}