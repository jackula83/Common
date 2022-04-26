using Common.Infra.MQ.RabbitMQ;
using Common.Infra.MQ.RabbitMQ.Connection;
using Common.Infra.MQ.UnitTests.Tests.RabbitMQ.Stubs;
using Moq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Infra.MQ.Tests.Abstracts
{
    public abstract class FxRabbitQueueTest
    {
        protected readonly Mock<IConnectionFactoryCreator> _connectionCreatorMock;
        protected readonly Mock<IConnection> _connectionMock;
        protected readonly Mock<IModel> _clientMock;
        protected readonly RabbitQueue _target;

        protected abstract IServiceProvider ServiceProvider { get; }

        public FxRabbitQueueTest()
        {
            _connectionCreatorMock = new();
            _connectionMock = new();
            _clientMock = new();
            _connectionCreatorMock.Setup(x => x.CreateConnection(It.IsAny<bool>())).Returns(_connectionMock.Object);
            _connectionMock.Setup(x => x.CreateModel()).Returns(_clientMock.Object);
            _target = new(this.ServiceProvider, _connectionCreatorMock.Object);
        }

        [Fact]
        public async virtual Task Publish_GivenAnEvent_PublishedWithExpectedBody()
        {
            // arrange
            var @event = new EventStub();

            // act
            await _target.Publish(@event);

            // assert
            _clientMock.Verify(x => x.BasicPublish(
                It.Is<string>(v => v == string.Empty),
                It.Is<string>(v => v == @event.Name),
                It.IsAny<bool>(),
                It.IsAny<IBasicProperties>(),
                It.Is<ReadOnlyMemory<byte>>(v => Encoding.UTF8.GetString(v.ToArray()) == JsonConvert.SerializeObject(@event)))
            , Times.Once);
        }

        [Fact]
        public async virtual Task Subscribe_WhenSubscribed_ConsumerStartedWithExpectedSettings()
        {
            // arrange
            var @event = new EventStub();

            // act
            await _target.Subscribe<EventStub, EventHandlerStub>();

            // assert
            _clientMock.Verify(x => x.BasicConsume(
                It.Is<string>(v => v == @event.Name),
                It.Is<bool>(v => v == false),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<IDictionary<string, object>>(),
                It.Is<IBasicConsumer>(v => v.Model == _clientMock.Object))
            , Times.Once);
        }

        [Fact]
        public async virtual Task ConsumerReceived_WhenEventArrivedForConsumer_HandlerIsInvoked()
        {
            // arrange
            var stub = new EventStub();
            var @event = new BasicDeliverEventArgs()
            {
                RoutingKey = stub.Name,
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(stub))
            };

            // act
            await _target.Subscribe<EventStub, EventHandlerStub>();

            // assert
            await Assert.ThrowsAsync<EventExceptionStub>(async () => await _target.OnConsumerReceived(this, @event));
        }
    }
}
