using Framework2.Application.UnitTests.Tests.Controllers.Stubs;
using Framework2.Domain.UnitTests.Models.Stubs;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Framework2.Application.UnitTests.Tests.Controllers
{
    public class FxEntityControllerTest
    {
        private readonly FxEntityControllerStub _target;
        private readonly ILogger<FxEntityControllerStub> _logger;
        private readonly Mock<IMediator> _mediatorMock;

        public FxEntityControllerTest()
        {
            _logger = new NullLogger<FxEntityControllerStub>();
            _mediatorMock = new();
            _target = new(_mediatorMock.Object, _logger);
        }

        [Fact]
        public async Task Get_Invoked_CallsMediatorWithNewRequest()
        {
            // act
            await _target.Get();

            // assert
            _mediatorMock.Verify(x => x.Send(
                It.Is<EntityQueryRequestStub>(s => s.Id == 0), 
                It.IsAny<CancellationToken>()));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetById_ProvideIdValue_CallsMediatorWithRequestWithId(int id)
        {
            // act
            await _target.GetById(id);

            // assert
            _mediatorMock.Verify(x => x.Send(
                It.Is<EntityQueryRequestStub>(s => s.Id == id),
                It.IsAny<CancellationToken>()));
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider))]
        public async Task Post_ProvideRequest_MediatorSendsExactRequest(EntityCommandRequestStub command)
        {
            // act
            await _target.Post(command);

            // assert
            _mediatorMock.Verify(x => x.Send(
                It.Is<EntityCommandRequestStub>(s => string.Compare(JsonConvert.SerializeObject(s), JsonConvert.SerializeObject(command)) == 0),
                It.IsAny<CancellationToken>()));
        }

        [Theory]
        [MemberData(nameof(CommandDataProvider))]
        public async Task Delete_ProvideRequest_MediatorSendsRequestWithTrueDeleteFlag(EntityCommandRequestStub command)
        {
            // act
            await _target.Delete(command);

            // assert
            _mediatorMock.Verify(x => x.Send(
                It.Is<EntityCommandRequestStub>(s => s.Item!.DeleteFlag == true),
                It.IsAny<CancellationToken>()));
        }

        private static IEnumerable<object[]> CommandDataProvider()
        {
            yield return new object[]
            {
                new EntityCommandRequestStub()
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    Item = new()
                    {
                        Id = 1,
                        Uuid = Guid.NewGuid(),
                        DeleteFlag = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = "a",
                        UpdatedBy = "b",
                    }
                }
            };
            yield return new object[]
            {
                new EntityCommandRequestStub()
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    Item = new()
                    {
                        Id = 2,
                        Uuid = Guid.NewGuid(),
                        DeleteFlag = false,
                        CreatedAt = default,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedBy = "a",
                        UpdatedBy = "b",
                    }
                }
            };
            yield return new object[]
            {
                new EntityCommandRequestStub()
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    Item = new()
                    {
                        Id = 3,
                        Uuid = Guid.NewGuid(),
                        DeleteFlag = true,
                        CreatedAt = default,
                        UpdatedAt = default,
                        CreatedBy = "a",
                        UpdatedBy = default,
                    }
                }
            };
        }
    }
}
