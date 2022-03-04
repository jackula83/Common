using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.MQ.Abstracts
{
    public abstract class EventQueue : IEventQueue
    {
        protected readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Event names -> List of handlers for the event
        /// </summary>
        protected readonly Dictionary<string, List<IEventHandler>> _eventHandlers;

        public EventQueue(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _eventHandlers = new();
        }

        public Task Publish<TEvent>(TEvent @event) where TEvent : FxEvent
        {
            throw new NotImplementedException();
        }

        public virtual Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent
            where TEventHandler : IEventHandler
        {
            throw new NotImplementedException();
        }
    }
}
