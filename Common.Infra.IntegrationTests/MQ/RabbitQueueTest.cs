using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Utilities;
using Common.Infra.IntegrationTests.Fixtures;
using Common.Infra.IntegrationTests.MQ.Stubs;
using Common.Infra.MQ.Environment;
using Common.Infra.MQ.Interfaces;
using Common.Infra.MQ.Queues;
using Common.Infra.MQ.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Common.Infra.IntegrationTests.MQ
{
    /// <summary>
    /// Only 1 basic test atm, want to implement basic ack before adding more tests
    /// </summary>
    public class RabbitQueueTest : IClassFixture<RabbitContainerFixture>
    {
        private readonly IEventQueue _target;
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<IEnvironment> _environmentMock;

        public RabbitQueueTest()
        {
            // initialise target
            _environmentMock = new();
            _environmentMock.Setup(x => x.Get(RabbitEnv.Hostname)).Returns("localhost");
            _environmentMock.Setup(x => x.Get(RabbitEnv.Username)).Returns(RabbitContainerFixture.RabbitUserName);
            _environmentMock.Setup(x => x.Get(RabbitEnv.Password)).Returns(RabbitContainerFixture.RabbitPassword);
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => _environmentMock.Object);
                svc.AddTransient<TestEventHandler>();
                svc.AddScoped<IConnectionFactoryCreator, ConnectionFactoryCreator>();
                svc.AddSingleton<IEventQueue, RabbitQueue>();
            });
            _target = _serviceProvider.GetRequiredService<IEventQueue>();
        }

        [Fact]
        public async Task Subscribe_PublishEvent_SubscriberConsumesEvent()
        {
            // arrange
            var @event = new TestEvent()
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _target.Subscribe<TestEvent, TestEventHandler>();
            var handlerInstance = await _target.GetHandler<TestEvent, TestEventHandler>();

            // act
            await _target.Publish(@event);

            while (await _target.Count<TestEvent>() > 0) ;
            while (!handlerInstance!.EventProcessed) ;

            // assert
            Assert.True(handlerInstance.EventProcessed);
        }
    }
}
