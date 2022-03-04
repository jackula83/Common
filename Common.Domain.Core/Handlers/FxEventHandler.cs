using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Handlers
{

    public abstract class FxEventHandler<TEvent> : IEventHandler<TEvent> 
        where TEvent : FxEvent
    {
        public virtual Type EventHandled => typeof(TEvent);

        public abstract Task Handle(TEvent @event);
    }
}
