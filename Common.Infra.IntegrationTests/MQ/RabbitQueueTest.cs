using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Utilities;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Infra.IntegrationTests.MQ
{
    public class RabbitQueueTest
    {
        private const string RabbitHostName = "host-rabbit";
        private const string RabbitUserName = "testUser";
        private const string RabbitPassword = "testPassword";

        private readonly IEventQueue _target;
        private readonly IServiceProvider _serviceProvider;
        private readonly DockerClient _docker;
        private readonly string _rabbitContainerId;
        private readonly Mock<ITestEventMonitor> _eventMonitorMock;
        private readonly Mock<IEnvironment> _environmentMock;

        public RabbitQueueTest()
        {
            // set up rabbitmq container
            _docker = new DockerClientConfiguration().CreateClient();
            _rabbitContainerId = this.CreateContainer().Result;

            // initialise target
            _eventMonitorMock = new();
            _environmentMock = new();
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => new EventHandlerStub());
                svc.AddTransient(_ => _environmentMock.Object);
                svc.AddTransient(_ => _eventMonitorMock.Object);
                svc.AddScoped<IConnectionFactoryCreator, ConnectionFactoryCreator>();
                svc.AddSingleton<IEventQueue, RabbitQueue>();
            });
            _target = _serviceProvider.GetRequiredService<IEventQueue>();
        }

        private async Task<string> CreateContainer()
        {
            // fetch the image
            await _docker.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = "rabbitmq",
                    Tag = "latest"
                },
                new AuthConfig(),
                new Progress<JSONMessage>()
            );

            // run the container
            var response = await _docker.Containers.CreateContainerAsync(new()
            {
                Image = "rabbitmq:latest",
                Cmd = new string[]
                {
                    "--hostname", RabbitHostName,
                    "--name", "test-rabbit",
                    "-e", $"RABBIT_DEFAULT_USER={RabbitUserName}",
                    "-e", $"RABBIT_DEFAULT_PASS={RabbitPassword}"
                }
            });

            if (await _docker.Containers.StartContainerAsync(response.ID, new()))
                return response.ID;

            throw new DockerApiException(
                HttpStatusCode.InternalServerError, 
                $"Could not start container {response.ID}, warnings: {string.Join(",", response.Warnings)}");
        }

        [Fact]
        public async Task Subscribe_PublishEvent_SubscriberConsumesEvent()
        {
            // arrange
            _environmentMock.Setup(x => x.Get(RabbitEnv.Hostname)).Returns(RabbitHostName);
            _environmentMock.Setup(x => x.Get(RabbitEnv.Username)).Returns(RabbitUserName);
            _environmentMock.Setup(x => x.Get(RabbitEnv.Password)).Returns(RabbitPassword);
            var @event = new TestEvent()
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await _target.Subscribe<TestEvent, TestEventHandler>();

            // act
            await _target.Publish(@event);

            // assert
            _eventMonitorMock.Verify(x => x.EventMonitored(@event));
        }
    }
}
