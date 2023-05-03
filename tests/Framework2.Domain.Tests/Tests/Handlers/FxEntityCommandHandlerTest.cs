using Framework2.Core.Extensions;
using Framework2.Domain.Core.Identity;
using Framework2.Domain.UnitTests.Handlers.Stubs;
using Framework2.Domain.UnitTests.Models.Stubs;
using Framework2.Infra.Data.UnitTests.Tests.Data.Stubs;
using Framework2.Infra.Data.UnitTests.Utilities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Framework2.Domain.UnitTests.Handlers
{
    public class FxEntityCommandHandlerTest
    {
        private readonly EntityCommandHandlerStub _target;
        private readonly Mock<IUserIdentity> _identityStub;
        private readonly SqlServerDbContextStub _context;
        private readonly EntityRepositoryStub _repository;
        private readonly ILogger<EntityCommandHandlerStub> _logger;
        private readonly Mock<IMediator> _mediatorMock;

        public FxEntityCommandHandlerTest()
        {
            _mediatorMock = new();
            _logger = new NullLogger<EntityCommandHandlerStub>();
            _identityStub = new Mock<IUserIdentity>();
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(FxEntityCommandHandlerTest), _mediatorMock.Object)!;
            _repository = new(_context);
            _target = new(_identityStub.Object, _logger, _repository);
        }

        [Fact]
        public async Task Handle_AddItemWithPermission_ItemAdded()
        {
            // arrange
            var username = "a";
            var request = new EntityCommandRequestStub { Item = new() };
            _identityStub.Setup(x => x.UserName).Returns(username);
            _target.WritePermission = true;

            // act
            var response = await _target.Handle(request);

            // assert
            Assert.NotNull(response);
            Assert.NotNull(response.Item);
            Assert.True(response.Success);
            Assert.Equal(response.Item!.CreatedBy, username);

            var savedItem = await _repository.Get(response.Item!.Id);
            Assert.NotNull(savedItem);
            Assert.Equal(response.Item!.Uuid, savedItem!.Uuid);
        }

        [Fact]
        public async Task Handle_UpdateItemWithPermission_ItemUpdated()
        {
            // arrange
            var username = "a";
            _identityStub.Setup(x => x.UserName).Returns("b");
            var entity = await _repository.Add(new());
            await _repository.Save();
            var request = new EntityCommandRequestStub { Item = entity.Tap(x => x.CreatedBy = username) };
            _target.WritePermission = true;

            // act
            var response = await _target.Handle(request);

            // assert
            Assert.NotNull(response);
            Assert.NotNull(response.Item);
            Assert.True(response.Success);
            Assert.Equal(response.Item!.CreatedBy, username);

            var savedItem = await _repository.Get(response.Item!.Id);
            Assert.NotNull(savedItem);
            Assert.Equal(response.Item!.Uuid, savedItem!.Uuid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task Handle_AddOrUpdateItemWithoutPermission_ExceptionThrown(int id)
        {
            // arrange
            _identityStub.Setup(x => x.UserName).Returns("a");
            _target.WritePermission = false;

            // assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _target.Handle(new() { Item = new() { Id = id } }));
        }
    }
}
