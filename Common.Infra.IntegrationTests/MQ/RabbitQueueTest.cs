using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Utilities;
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
using System.Text;
using System.Threading.Tasks;

namespace Common.Infra.IntegrationTests.MQ
{
    public class RabbitQueueTest
    {
        private const string RabbitUserName = "testUser";
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

            // initialise target
            _environmentMock = new();
            _serviceProvider = Utils.CreateServiceProvider(svc =>
            {
                svc.AddTransient(_ => new EventHandlerStub());
                svc.AddTransient(_ => _environmentMock.Object);
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
            var createParams = new CreateContainerParameters()
            {
                Image = "rabbitmq:latest",
                Cmd = new string[]
                {
                    "--hostname", "host-rabbit",
                    "--name", "test-rabbit",
                    "-e", $"RABBIT_DEFAULT_USER={RabbitUserName}",
                    "-e", $"RABBIT_DEFAULT_PASS={RabbitPassword}"
                }
            };
            var response = await _docker.Containers.CreateContainerAsync(createParams);
            return response.ID;
        }
    }
}
