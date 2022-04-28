namespace Framework2.Infra.MQ.Core
{
    public abstract class FxEventHandler
    {
        public abstract Type EventHandled { get; }
    }

    public abstract class FxEventHandler<TEvent> : FxEventHandler
        where TEvent : FxEvent
    {
        public override Type EventHandled => typeof(TEvent);

        public abstract Task Handle(TEvent @event, CancellationToken cancellationToken = default);
    }
}
