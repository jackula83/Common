using Common.Domain.Core.Handlers;
using Common.Domain.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Common.Infra.MQ.Tests.Queues.Stubs
{
    public sealed class EventHandlerStub : FxEventHandler<EventStub>, IEventHandler
    {
        public override Task Handle(EventStub @event)
        {
            throw new EventExceptionStub();
        }
    }
}
