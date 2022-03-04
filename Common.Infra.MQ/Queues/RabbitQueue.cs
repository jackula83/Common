using Common.Domain.Core.Interfaces;
using Common.Infra.MQ.Environment;
using Common.Infra.MQ.Interfaces;
using Common.Infra.MQ.Queues.Abstracts;
using Common.Infra.MQ.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Common.Infra.MQ.Queues
{
    public sealed class RabbitQueue : FxEventQueue, IEventQueue
    {
        private readonly IConnectionFactoryCreator _connectionCreator;

        public RabbitQueue(IServiceProvider serviceProvider, IConnectionFactoryCreator creator) 
            : base(serviceProvider)
        {
            _connectionCreator = creator;
        }

        public override async Task Publish<TEvent>(TEvent @event)
        {
            await Task.CompletedTask;

            using var connection = _connectionCreator.CreateConnection();
            using var client = connection.CreateModel();

            this.QueueDeclare(client, @event.Name);

            var payload = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(payload);

            // publish to rabbitmq
            client.BasicPublish(String.Empty, @event.Name, default, body);
        }

        protected override async Task StartConsumingEvents<TEvent>(string eventName)
        {
            await Task.CompletedTask;

            using var connection = _connectionCreator.CreateConnection(true);
            using var client = connection.CreateModel();

            this.QueueDeclare(client, eventName);

            var consumer = new AsyncEventingBasicConsumer(client);
            consumer.Received += OnConsumerReceived;

            // start the consumer
            client.BasicConsume(eventName, true, consumer);
        }

        public async Task OnConsumerReceived(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var payload = Encoding.UTF8.GetString(@event.Body.Span);

            await this.ConsumeEvent(eventName, payload);
        }

        private QueueDeclareOk QueueDeclare(IModel clientModel, string eventName)
            => clientModel.QueueDeclare(eventName, false, false, false, null);
    }
}
