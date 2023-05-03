using Framework2.Infra.Data.Entity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Handlers
{
    public abstract class FxDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : FxDomainEvent, INotification
    {
        protected readonly ILogger<FxDomainEventHandler<TDomainEvent>> _logger;

        public FxDomainEventHandler(ILogger<FxDomainEventHandler<TDomainEvent>> logger)
        {
            _logger = logger;   
        }

        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken = default)
        {
            await this.Execute(notification, cancellationToken);
        }

        protected abstract Task Execute(TDomainEvent @event, CancellationToken cancellationToken);
    }
}
