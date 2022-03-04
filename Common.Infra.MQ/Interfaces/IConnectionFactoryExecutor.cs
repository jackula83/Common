using RabbitMQ.Client;

namespace Common.Infra.MQ.Interfaces
{
    public interface IConnectionFactoryExecutor
    {
        void Execute(Action<IModel> clientLogic, string hostname, bool dispatchConsumersAsync = false);
    }
}
