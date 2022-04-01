using Common.Domain.Core.Handlers;
using Common.Domain.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Common.Infra.IntegrationTests.MQ.Stubs
{
    public sealed class TestEventHandler : FxEventHandler<TestEvent>, IEventHandler
    {
        private readonly ITestEventMonitor _testEventMonitor;

        public TestEventHandler(ITestEventMonitor eventMonitor)
        {
            _testEventMonitor = eventMonitor;
        }

        public override async Task Handle(TestEvent @event)
        {
            await Task.CompletedTask;
            _testEventMonitor.EventMonitored(@event.CorrelationId);
        }
    }
}
