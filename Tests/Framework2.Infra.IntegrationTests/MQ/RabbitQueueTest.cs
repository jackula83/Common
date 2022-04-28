using Framework2.Domain.Tests.Utilities;
using Framework2.Infra.IntegrationTests.Fixtures;
using Framework2.Infra.IntegrationTests.MQ.Stubs;
using Framework2.Infra.MQ.Core;
using Framework2.Infra.MQ.RabbitMQ;
using Framework2.Infra.MQ.RabbitMQ.Connection;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Framework2.Infra.IntegrationTests.MQ
{
    /// <summary>
    /// Only 1 basic test atm, want to implement basic ack before adding more tests
    /// </summary>
    public class RabbitQueueTest : IDisposable, IClassFixture<RabbitContainerFixture>
    {
        private readonly IEventQueue _target;
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<ITestEventMonitor> _eventMonitorMock;
        private readonly RabbitContainerFixture _rabbitFixture;
        private readonly ConnectionFactoryConfig _connectionFactoryConfig;

        public RabbitQueueTest(RabbitContainerFixture rabbitFixture)
        {
            _rabbitFixture = rabbitFixture;
            _eventMonitorMock = new();
            _connectionFactoryConfig = new()
            {
                HostName = "localhost",
                UserName = RabbitContainerFixture.RabbitUserName,
                Password = RabbitContainerFixture.RabbitPassword
            };
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => _eventMonitorMock.Object);
                svc.AddTransient<TestEventHandler>();
                svc.AddScoped(_ => _connectionFactoryConfig);
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
