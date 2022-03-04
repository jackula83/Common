using Common.Infra.MQ.Interfaces;
using RabbitMQ.Client;

namespace Common.Infra.MQ.Services
{
    public sealed class ConnectionFactoryExecutor : IConnectionFactoryExecutor
    {
        public void Execute(Action<IModel> clientLogic, string hostname, bool dispatchConsumersAsync = false)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                DispatchConsumersAsync = dispatchConsumersAsync
            };

            using var conn = factory.CreateConnection();
            using var client = conn.CreateModel();

            clientLogic(client);
        }
    }
}
