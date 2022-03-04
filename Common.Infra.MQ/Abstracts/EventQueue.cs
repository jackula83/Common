using Common.Domain.Core.Handlers;
using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Common.Infra.MQ.Abstracts
{
    /// <summary>
    /// The base event queue, derived services should have a singleton scope
    /// </summary>
    public abstract class EventQueue : IEventQueue
    {
        protected readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Event names -> List of handlers for the event
        /// </summary>
        protected readonly Dictionary<string, List<IEventHandler>> _eventHandlers;

        protected abstract Task StartConsumingEvents<TEvent>()
            where TEvent : FxEvent, new();

        public EventQueue(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventHandlers = new();
        }

        public abstract Task Publish<TEvent>(TEvent @event) 
            where TEvent : FxEvent, new();

        public virtual async Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent, new()
            where TEventHandler : IEventHandler
        {
            var eventName = new TEvent().Name;
            var handler = _serviceProvider.GetRequiredService<TEventHandler>();

            if (!_eventHandlers.ContainsKey(eventName))
                _eventHandlers[eventName] = new();

            if (!_eventHandlers[eventName].Any(x => x.GetType() == handler.GetType()))
                _eventHandlers[eventName].Add(handler);

            await this.StartConsumingEvents<TEvent>();
        }

        protected virtual async Task ConsumeEvent(string eventName, string payload)
        {
            if (!_eventHandlers.ContainsKey(eventName))
                return;

            var handlers = _eventHandlers[eventName];
            foreach (var handler in handlers)
            {
                var @event = JsonConvert.DeserializeObject(payload, handler.EventHandled);
                var genericType = typeof(IEventHandler<>).MakeGenericType(handler.EventHandled);
                var handlerName = nameof(IEventHandler<FxEvent>.Handle);

                await (Task)genericType.GetMethod(handlerName)!.Invoke(handler, new[] { @event })!;
            }
        }
    }
}
