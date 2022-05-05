using Framework2.Infra.MQ.Tests.Abstracts;
using Framework2.Infra.MQ.UnitTests.Tests.RabbitMQ.Stubs;
using Framework2.Tests.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework2.Infra.MQ.UnitTests.Tests.RabbitMQ
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
