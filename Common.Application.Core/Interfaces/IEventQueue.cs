using Common.Application.Core.Handlers;
using Common.Application.Core.Models;

namespace Common.Application.Core.Interfaces
{
    public interface IEventQueue
    {
        Task Publish<TEvent>(TEvent @event) 
            where TEvent : FxEvent;
        Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent
            where TEventHandler : FxEventHandler<TEvent>;
        Task<uint> Count<TEvent>()
            where TEvent : FxEvent;
    }
}
