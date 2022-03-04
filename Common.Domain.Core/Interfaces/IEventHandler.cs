using Common.Domain.Core.Models;

namespace Common.Domain.Core.Interfaces
{
    public interface IEventHandler { }

    public interface IEventHandler<TEvent> : IEventHandler
        where TEvent : FxEvent
    {
        string EventHandled { get; }

        Task Handle(TEvent @event);
    }
}
