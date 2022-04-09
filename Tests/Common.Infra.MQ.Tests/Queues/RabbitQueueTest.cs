using Common.Domain.Tests.Utilities;
using Common.Infra.MQ.Tests.Abstracts;
using Common.Infra.MQ.Tests.Queues.Stubs;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Infra.MQ.Tests.Queues
{
    public class RabbitQueueTest : FxRabbitQueueTest
    {
        protected override IServiceProvider ServiceProvider 
            => Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => new EventHandlerStub());
            });
    }
}
