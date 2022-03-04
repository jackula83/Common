using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.MQ
{
    public sealed class KafkaMQ : IEventQueue
    {
        public Task Publish<TEvent>(TEvent @event) where TEvent : FxEvent
        {
            throw new NotImplementedException();
        }

        public Task Subscribe<TEvent, TEventHandler>()
            where TEvent : FxEvent
            where TEventHandler : IEventHandler
        {
            throw new NotImplementedException();
        }
    }
}
