using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Utilities;
using Common.Infra.IntegrationTests.Fixtures;
using Common.Infra.IntegrationTests.MQ.Stubs;
using Common.Infra.MQ.Environment;
using Common.Infra.MQ.Interfaces;
using Common.Infra.MQ.Queues;
using Common.Infra.MQ.Services;
using Common.Infra.MQ.Tests.Queues.Stubs;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Common.Infra.IntegrationTests.MQ
{
    /// <summary>
    /// Can only support 1 test, move CreateContainer to project initialisation
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
