using Common.Domain.Core.Handlers;
using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;

namespace Common.Infra.MQ.Queues.Abstracts
{
    /// <summary>
    /// The base event queue, derived services should have a singleton scope
    /// </summary>
    public abstract class FxEventQueue : IEventQueue
    {
        protected readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Event names -> List of handlers for the event
        /// </summary>
        protected readonly Dictionary<string, List<FxEventHandler>> _eventHandlers;

        public abstract Task<uint> Count<TEvent>()
            where TEvent : FxEvent, new();

        protected abstract Task StartConsumingEvents<TEvent>(string eventName)
            where TEvent : FxEvent, new();

        public FxEventQueue(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _eventHandlers = new();
        }

        public abstract Task Publish<TEvent>(TEvent @event)
            where TEvent : FxEvent, new();

        public virtual async Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent, new()
            where TEventHandler : FxEventHandler<TEvent>
        {
            var eventName = new TEvent().Name;
            var handler = _serviceProvider.GetRequiredService<TEventHandler>();

            if (!_eventHandlers.ContainsKey(eventName))
                _eventHandlers[eventName] = new();

            if (!_eventHandlers[eventName].Any(x => x.GetType() == handler.GetType()))
                _eventHandlers[eventName].Add(handler);

            await StartConsumingEvents<TEvent>(eventName);
        }

        protected virtual async Task ConsumeEvent(string eventName, string payload)
        {
            if (!_eventHandlers.ContainsKey(eventName))
                return;

            var handlers = _eventHandlers[eventName];
            foreach (var handler in handlers)
            {
                var @event = JsonConvert.DeserializeObject(payload, handler.EventHandled);
                var genericType = typeof(FxEventHandler<>).MakeGenericType(handler.EventHandled);
                var handlerName = nameof(FxEventHandler<FxEvent>.Handle);

                try
                {
                    await (Task)genericType.GetMethod(handlerName)!.Invoke(handler, new[] { @event })!;
                }
                catch (TargetInvocationException ex)
                {
                    /// throw the actual exception from the handler, since reflection by default hides it under <see cref="TargetInvocationException"/>
                    throw ex.InnerException!;
                }
            }
        }
    }
}
