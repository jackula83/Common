using Framework2.Infra.MQ.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Framework2.Infra.MQ.UnitTests.Tests.RabbitMQ.Stubs
{
    public sealed class EventHandlerStub : FxEventHandler<EventStub>
    {
        public override Task Handle(EventStub @event, CancellationToken cancellationToken = default)
        {
            throw new EventExceptionStub();
        }
    }
}
