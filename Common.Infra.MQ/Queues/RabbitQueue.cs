using Common.Domain.Core.Interfaces;
using Common.Infra.MQ.Environment;
using Common.Infra.MQ.Interfaces;
using Common.Infra.MQ.Queues.Abstracts;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Common.Infra.MQ.Queues
{
    public sealed class RabbitQueue : FxEventQueue, IEventQueue
    {
        private readonly IEnvironment _env;
        private readonly IConnectionFactoryExecutor _factoryExecutor;

        public RabbitQueue(IServiceProvider serviceProvider, IEnvironment env, IConnectionFactoryExecutor executor) 
            : base(serviceProvider)
        {
            _env = env;
            _factoryExecutor = executor;
        }

        public override async Task Publish<TEvent>(TEvent @event)
        {
            await Task.CompletedTask;

            _factoryExecutor.Execute(
                hostname: _env.Get(RabbitEnv.Hostname)!,
                clientLogic: client => 
                {
                    this.QueueDeclare(client, @event.Name);

                    var payload = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(payload);

                    client.BasicPublish(String.Empty, @event.Name, default, body);
                });
        }

        protected override async Task StartConsumingEvents<TEvent>(string eventName)
        {
            await Task.CompletedTask;

            _factoryExecutor.Execute(
                hostname: _env.Get(RabbitEnv.Hostname)!,
                dispatchConsumersAsync: true,
                clientLogic: client =>
                {
                    this.QueueDeclare(client, eventName);

                    var consumer = new AsyncEventingBasicConsumer(client);
                    consumer.Received += Consumer_Received;
                    client.BasicConsume(eventName, true, consumer);
                });
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var eventName = @event.RoutingKey;
            var payload = Encoding.UTF8.GetString(@event.Body.Span);

            await this.ConsumeEvent(eventName, payload);
        }

        private QueueDeclareOk QueueDeclare(IModel clientModel, string eventName)
            => clientModel.QueueDeclare(eventName, false, false, false, null);
    }
}
