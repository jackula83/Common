using Common.Domain.Core.Models;

namespace Common.Domain.Core.Interfaces
{
    public interface IEventHandler
    {
        Type EventHandled { get; }
    }

    public interface IEventHandler<TEvent> : IEventHandler
        where TEvent : FxEvent
    {
        Task Handle(TEvent @event);
    }
}
