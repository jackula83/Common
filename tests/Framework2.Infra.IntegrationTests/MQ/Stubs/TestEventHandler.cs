using Framework2.Infra.MQ.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Framework2.Infra.IntegrationTests.MQ.Stubs
{
    public sealed class TestEventHandler : FxEventHandler<TestEvent>
    {
        private readonly ITestEventMonitor _testEventMonitor;

        public TestEventHandler(ITestEventMonitor eventMonitor)
        {
            _testEventMonitor = eventMonitor;
        }

        public override async Task Handle(TestEvent @event, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            _testEventMonitor.EventMonitored(@event.CorrelationId);
        }
    }
}
