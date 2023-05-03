using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework2.Infra.Data.Entity
{
    public abstract class FxAggregateRoot : FxDataObject
    {
        private List<FxDomainEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<FxDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : FxDomainEvent
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
