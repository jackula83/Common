using Docker.DotNet;
using Docker.DotNet.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Infra.IntegrationTests.Fixtures
{
    public class RabbitContainerFixture : IDisposable
    {
        private readonly DockerClient _docker;
        private readonly string _rabbitContainerId;

        public const string RabbitHostName = "host-rabbit";
        public const string RabbitUserName = "testUsername";
        public const string RabbitPassword = "testPassword";

        private const string RabbitCompletionText = "Server startup complete";
        private const string RabbitContainerName = "test-integration-rabbit";

        public RabbitContainerFixture()
        {
            _docker = new DockerClientConfiguration().CreateClient();
            this.RemoveExistingContainer().Wait();
            _rabbitContainerId = this.CreateContainer().Result;

            var cancellationToken = new CancellationTokenSource();
            var containerInitProgress = new Progress<string>(message =>
            {
                if (message.Contains(RabbitCompletionText))
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
        }

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

            // run the container
            var servicePort = Protocols.DefaultProtocol.DefaultPort.ToString();
            var managementPort = "15672";
            var response = await _docker.Containers.CreateContainerAsync(new()
            {
                Image = $"rabbitmq:{tag}",
                Hostname = RabbitHostName,
                Name = RabbitContainerName,
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

        private async Task RemoveExistingContainer()
        {
            var parameters = new ContainersListParameters
            {
                All = true,
            };
            var containers = await _docker.Containers.ListContainersAsync(parameters);
            var containerId = containers.FirstOrDefault(
                container => container.Names.FirstOrDefault(
                    name => string.Compare(name.Replace("/", String.Empty), RabbitContainerName, true) == 0
                ) != default
            )?.ID;

            if (!string.IsNullOrEmpty(containerId))
                await this.RemoveContainer(containerId);
        }

        private async Task RemoveContainer(string containerId)
        {
            await _docker.Containers.StopContainerAsync(
                containerId,
                new() { WaitBeforeKillSeconds = 30 });

            await _docker.Containers.RemoveContainerAsync(containerId, new());
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_rabbitContainerId))
                this.RemoveContainer(_rabbitContainerId).Wait();
        }
    }
}
