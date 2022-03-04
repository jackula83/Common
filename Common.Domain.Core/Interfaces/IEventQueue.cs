using Common.Domain.Core.Handlers;
using Common.Domain.Core.Models;

namespace Common.Domain.Core.Interfaces
{
    public interface IEventQueue
    {
        Task Publish<TEvent>(TEvent @event) 
            where TEvent : FxEvent, new();
        Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent, new()
            where TEventHandler : FxEventHandler<TEvent>;
    }
}
