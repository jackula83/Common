using Common.Domain.Core.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Infra.MQ.UnitTests.Tests.RabbitMQ.Stubs
{
    public sealed class EventHandlerStub : FxEventHandler<EventStub>
    {
        public override Task Handle(EventStub @event, CancellationToken cancellationToken = default)
        {
            throw new EventExceptionStub();
        }
    }
}
