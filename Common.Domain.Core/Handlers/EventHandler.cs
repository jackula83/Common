using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Handlers
{
    public abstract class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : FxEvent
    {
        public virtual string EventHandled => typeof(TEvent).Name;

        public abstract Task Handle(TEvent @event);
    }
}
