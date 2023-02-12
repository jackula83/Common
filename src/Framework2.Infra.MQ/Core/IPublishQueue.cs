namespace Framework2.Infra.MQ.Core
{
    public interface IPublishQueue
    {
        Task Publish<TEvent>(TEvent @event)
            where TEvent : FxEvent;
    }
}
