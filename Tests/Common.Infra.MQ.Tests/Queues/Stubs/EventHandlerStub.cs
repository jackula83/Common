using Common.Application.Core.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Infra.MQ.Tests.Queues.Stubs
{
    public sealed class EventHandlerStub : FxEventHandler<EventStub>
    {
        public override Task Handle(EventStub @event, CancellationToken cancellationToken = default)
        {
            throw new EventExceptionStub();
        }
    }
}
