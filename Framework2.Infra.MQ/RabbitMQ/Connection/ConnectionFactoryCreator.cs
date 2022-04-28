using RabbitMQ.Client;

namespace Framework2.Infra.MQ.RabbitMQ.Connection
{
    public class ConnectionFactoryCreator : IConnectionFactoryCreator
    {
        private readonly ConnectionFactoryConfig _config;

        public ConnectionFactoryCreator(ConnectionFactoryConfig config)
        {
            _config = config;
        }

        public IConnection CreateConnection(bool dispatchConsumersAsync = false)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.HostName,
                UserName = _config.UserName,
                Password = _config.Password,
                VirtualHost = "/",
                Port = Protocols.DefaultProtocol.DefaultPort,
                DispatchConsumersAsync = dispatchConsumersAsync
            };

            return factory.CreateConnection();
        }
    }
}
