using Dapr.Client;
using Framework2.Infra.MQ.Core;

namespace Framework2.Infra.MQ.DaprMQ
{
    public class DaprQueue : IPublishQueue
    {
        private readonly DaprClient _client;

        public DaprQueue()
        {
            _client = new DaprClientBuilder().Build();
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : FxEvent
        {
            await _client.PublishEventAsync(DaprConfig.QueueName, @event.Name, @event);
        }
    }
}
