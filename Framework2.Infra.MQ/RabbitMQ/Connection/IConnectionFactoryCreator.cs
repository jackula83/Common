using RabbitMQ.Client;

namespace Framework2.Infra.MQ.RabbitMQ.Connection
{
    public interface IConnectionFactoryCreator
    {
        IConnection CreateConnection(bool dispatchConsumersAsync = false);
    }
}
