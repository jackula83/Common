using Common.Application.Core.Interfaces;
using Common.Domain.Core.Interfaces;
using Common.Infra.MQ.Queues.Abstracts;

namespace Common.Infra.MQ.Queues
{
    public sealed class KafkaQueue : FxEventQueue, IEventQueue
    {
        public KafkaQueue(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task<uint> Count<TEvent>()
        {
            throw new NotImplementedException();
        }

        public override Task Publish<TEvent>(TEvent @event)
        {
            throw new NotImplementedException();
        }

        protected override Task StartConsumingEvents<TEvent>(string eventName)
        {
            throw new NotImplementedException();
        }
    }
}
