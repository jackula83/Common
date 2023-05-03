using MediatR;

namespace Framework2.Infra.Data.Entity
{
    public abstract class FxAggregateRoot : FxDataObject
    {
        private readonly IMediator _mediator;

        public FxAggregateRoot(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected void Apply<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : FxDomainEvent, INotification
            => _mediator.Publish(@event, cancellationToken);
    }
}
