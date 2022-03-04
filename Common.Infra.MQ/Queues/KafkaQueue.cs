using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using Common.Infra.MQ.Queues.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.MQ.Queues
{
    public sealed class KafkaQueue : FxEventQueue, IEventQueue
    {
        public KafkaQueue(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task Publish<TEvent>(TEvent @event)
        {
            throw new NotImplementedException();
        }

        protected override Task StartConsumingEvents<TEvent>(string eventName)
        {
            throw new NotImplementedException();
        }
    }
}
