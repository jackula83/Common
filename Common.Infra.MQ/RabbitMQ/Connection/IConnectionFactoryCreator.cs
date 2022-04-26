using RabbitMQ.Client;

namespace Common.Infra.MQ.RabbitMQ.Connection
{
    public interface IConnectionFactoryCreator
    {
        IConnection CreateConnection(bool dispatchConsumersAsync = false);
    }
}
