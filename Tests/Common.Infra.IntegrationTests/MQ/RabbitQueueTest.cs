using Common.Application.Core.Interfaces;
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
    public class RabbitQueueTest : IDisposable, IClassFixture<RabbitContainerFixture>
    {
        private readonly IEventQueue _target;
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<ITestEventMonitor> _eventMonitorMock;
        private readonly Mock<IEnvironment> _environmentMock;
        private readonly RabbitContainerFixture _rabbitFixture;

        public RabbitQueueTest(RabbitContainerFixture rabbitFixture)
        {
            _rabbitFixture = rabbitFixture;
            _eventMonitorMock = new();
            _environmentMock = new();
            _environmentMock.Setup(x => x.Get(RabbitEnv.Hostname)).Returns("localhost");
            _environmentMock.Setup(x => x.Get(RabbitEnv.Username)).Returns(RabbitContainerFixture.RabbitUserName);
            _environmentMock.Setup(x => x.Get(RabbitEnv.Password)).Returns(RabbitContainerFixture.RabbitPassword);
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => _environmentMock.Object);
                svc.AddTransient(_ => _eventMonitorMock.Object);
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

            // act
            await _target.Subscribe<TestEvent, TestEventHandler>();
            await _target.Publish(@event);
            while (await _target.Count<TestEvent>() > 0) ;

            // assert
            _eventMonitorMock.Verify(x => x.EventMonitored(@event.CorrelationId));
        }

        [Fact]
        public async Task Subscribe_NeverPublishEvent_SubscriberDoesNotConsumeEvent()
        {
            // act
            await _target.Subscribe<TestEvent, TestEventHandler>();

            // assert
            _eventMonitorMock.Verify(x => x.EventMonitored(It.IsAny<string>()), Times.Never());
        }

        public void Dispose()
        {
            try
            {
                _rabbitFixture.ResetContainer().Wait();
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}
