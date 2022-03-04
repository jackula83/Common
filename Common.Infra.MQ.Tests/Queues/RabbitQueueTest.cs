using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Utilities;
using Common.Infra.MQ.Environment;
using Common.Infra.MQ.Interfaces;
using Common.Infra.MQ.Queues;
using Common.Infra.MQ.Tests.Queues.Stubs;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Common.Infra.MQ.Tests.Queues
{
    public class RabbitQueueTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Mock<IEnvironment> _environmentMock;
        private readonly Mock<IConnectionFactoryExecutor> _connectionFactoryExecutorMock;
        private readonly RabbitQueue _target;

        public RabbitQueueTest()
        {
            _environmentMock = new();
            _connectionFactoryExecutorMock = new();
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => new EventHandlerStub());
            });
            _target = new(_serviceProvider, _environmentMock.Object, _connectionFactoryExecutorMock.Object);
        }

        [Fact]
        public async Task Publish_GivenAnEvent_PublishedWithCorrectHost()
        {
            // arrange
            var hostname = "a";
            var @event = new EventStub();

            _environmentMock.Setup(x => x.Get(RabbitEnv.Hostname)).Returns(hostname);

            // act
            await _target.Publish(@event);

            // assert
            _connectionFactoryExecutorMock.Verify(x => x.Execute(
                It.IsAny<Action<IModel>>(),
                It.Is<string>(v => v == hostname),
                It.Is<bool>(v => v == false))
            , Times.Once);
        }

        [Fact]
        public async Task Subscribe_WhenSubscribed_ConsumedWithCorrectHostAndAsyncConsume()
        {
            // arrange
            var hostname = "a";
            var @event = new EventStub();

            _environmentMock.Setup(x => x.Get(RabbitEnv.Hostname)).Returns(hostname);

            // act
            await _target.Subscribe<EventStub, EventHandlerStub>();

            // assert
            _connectionFactoryExecutorMock.Verify(x => x.Execute(
                It.IsAny<Action<IModel>>(),
                It.Is<string>(v => v == hostname),
                It.Is<bool>(v => v == true))
            , Times.Once);
        }
    }
}
