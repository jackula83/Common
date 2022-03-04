using RabbitMQ.Client;

namespace Common.Infra.MQ.Interfaces
{
    public interface IConnectionFactoryCreator
    {
        IConnection CreateConnection(bool dispatchConsumersAsync = false);
    }
}
