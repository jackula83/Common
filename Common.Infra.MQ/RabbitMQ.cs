using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;

namespace Common.Infra.MQ
{
    public sealed class RabbitMQ : IEventQueue
    {
        public Task Publish<TEvent>(TEvent @event) where TEvent : FxEvent
        {
            throw new NotImplementedException();
        }

        public Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent
            where TEventHandler : IEventHandler
        {
            throw new NotImplementedException();
        }
    }
}
