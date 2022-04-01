using Common.Domain.Core.Handlers;
using Common.Domain.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Common.Infra.IntegrationTests.MQ.Stubs
{
    public sealed class TestEventHandler : FxEventHandler<TestEvent>, IEventHandler
    {
        public bool EventProcessed { get; set; } = false;

        public TestEventHandler()
        {
        }

        public override async Task Handle(TestEvent @event)
        {
            await Task.CompletedTask;
            this.EventProcessed = true;
        }
    }
}
