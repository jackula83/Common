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
    public class RabbitQueueTest : IDisposable
    {
        private const string RabbitHostName = "host-rabbit";
        private const string RabbitUserName = "testUsername";
        private const string RabbitPassword = "testPassword";

        private readonly IEventQueue _target;
        private readonly IServiceProvider _serviceProvider;
        private readonly DockerClient _docker;
        private readonly string _rabbitContainerId;
        private readonly Mock<IEnvironment> _environmentMock;

        public RabbitQueueTest()
        {
            // set up rabbitmq container
            _docker = new DockerClientConfiguration().CreateClient();
            _rabbitContainerId = this.CreateContainer().Result;

            var cancellationToken = new CancellationTokenSource();
            var containerInitProgress = new Progress<string>(message =>
                {
                    if (message.Contains("Server startup complete"))
                        cancellationToken.Cancel();
                });
            var containerLogParams = new ContainerLogsParameters
            {
                Follow = true,
                ShowStdout = true,
                ShowStderr = true,
            };
            try
            {
                _docker.Containers.GetContainerLogsAsync(_rabbitContainerId, containerLogParams, cancellationToken.Token, containerInitProgress).Wait();
            }
            catch (AggregateException ex) 
            {
                if (!(ex.InnerException != null && ex.InnerException is TaskCanceledException))
                    throw ex.InnerException!;
            }

            // initialise target
            _environmentMock = new();
            _environmentMock.Setup(x => x.Get(RabbitEnv.Hostname)).Returns("localhost");
            _environmentMock.Setup(x => x.Get(RabbitEnv.Username)).Returns(RabbitUserName);
            _environmentMock.Setup(x => x.Get(RabbitEnv.Password)).Returns(RabbitPassword);
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => _environmentMock.Object);
                svc.AddTransient<TestEventHandler>();
                svc.AddScoped<IConnectionFactoryCreator, ConnectionFactoryCreator>();
                svc.AddSingleton<IEventQueue, RabbitQueue>();
            });
            _target = _serviceProvider.GetRequiredService<IEventQueue>();
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_rabbitContainerId))
            {
                _docker.Containers.StopContainerAsync(
                    _rabbitContainerId,
                    new()
                    {
                        WaitBeforeKillSeconds = 30
                    }).Wait();

                // commenting this line doing this is good for diagnostics
                _docker.Containers.PruneContainersAsync().Wait();
            }
        }

        /// <summary>
        /// TODO: Move this to a fixture
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DockerApiException"></exception>
        private async Task<string> CreateContainer()
        {
            // management is good for diagnosis
            var tag = "3-management";

            // fetch the image
            await _docker.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = "rabbitmq",
                    Tag = tag
                },
                new AuthConfig(),
                new Progress<JSONMessage>()
            );

            // TODO: add some safety here to kill existing integration test rabbitmq container
            // run the container
            var servicePort = Protocols.DefaultProtocol.DefaultPort.ToString();
            var managementPort = "15672";
            var response = await _docker.Containers.CreateContainerAsync(new()
            {
                Image = $"rabbitmq:{tag}",
                Hostname = RabbitHostName,
                Name = $"test-rabbit-{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16)}",
                Env = new string[]
                {
                    $"RABBITMQ_DEFAULT_USER={RabbitUserName}",
                    $"RABBITMQ_DEFAULT_PASS={RabbitPassword}",
                },
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { servicePort, new() },
                    { managementPort, new() }
                },
                HostConfig = new()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { servicePort, new List<PortBinding> { new PortBinding { HostPort = servicePort } } },
                        { managementPort, new List<PortBinding> { new PortBinding { HostPort = managementPort } } },
                    }
                },
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
