using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Handlers
{
    public abstract class FxEventHandler 
    {
        public abstract Type EventHandled { get; }
    }

    public abstract class FxEventHandler<TEvent> : FxEventHandler
        where TEvent : FxEvent
    {
        public override Type EventHandled => typeof(TEvent);

        public abstract Task Handle(TEvent @event);
    }
}
