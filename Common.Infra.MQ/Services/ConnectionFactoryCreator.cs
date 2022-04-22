using Common.Application.Core.Interfaces;
using Common.Domain.Core.Interfaces;
using Common.Infra.MQ.Environment;
using Common.Infra.MQ.Interfaces;
using RabbitMQ.Client;

namespace Common.Infra.MQ.Services
{
    public sealed class ConnectionFactoryCreator : IConnectionFactoryCreator
    {
        private readonly IEnvironment _environment;

        public ConnectionFactoryCreator(IEnvironment environment)
        {
            _environment = environment;
        }

        public IConnection CreateConnection(bool dispatchConsumersAsync = false)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _environment.Get(RabbitEnv.Hostname),
                UserName = _environment.Get(RabbitEnv.Username),
                Password = _environment.Get(RabbitEnv.Password),
                VirtualHost = "/",
                Port = Protocols.DefaultProtocol.DefaultPort,
                DispatchConsumersAsync = dispatchConsumersAsync
            };

            return factory.CreateConnection();
        }
    }
}
