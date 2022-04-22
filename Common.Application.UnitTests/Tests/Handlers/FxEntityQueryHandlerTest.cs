using Common.Application.UnitTests.Tests.Handlers.Stubs;
using Common.Domain.Tests.Utilities;
using Common.Domain.UnitTests.Tests.Data.Stubs;
using Common.Domain.UnitTests.Tests.Models.Stubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Common.Application.UnitTests.Tests.Handlers
{
    public class FxEntityQueryHandlerTest
    {
        private readonly EntityQueryHandlerStub _target;
        private readonly SqlServerDbContextStub _context;
        private readonly EntityRepositoryStub _repository;
        private readonly ILogger<EntityQueryHandlerStub> _logger;

        public FxEntityQueryHandlerTest()
        {
            _logger = new NullLogger<EntityQueryHandlerStub>();
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(FxEntityQueryHandlerTest))!;
            _repository = new(_context);
            _target = new(_logger, _repository);
        }

        [Fact]
        public async Task Handle_DefaultRequestSent_ReturnsEntityCollection()
        {
            // arrange
            var (entity1, entity2) = await this.InitialiseRepository();

            // act
            var result = await _target.Handle(new());

            // assert
            Assert.NotNull(result);
            Assert.True(result.Items?.Count == 2);
            Assert.True(result.Items!.FirstOrDefault(x => x.Uuid == entity1.Uuid) != null);
            Assert.True(result.Items!.FirstOrDefault(x => x.Uuid == entity2.Uuid) != null);
        }

        [Fact]
        public async Task Handle_SpecificRequestSent_ReturnsSpecificEntity()
        {
            // arrange
            var (entity1, _) = await this.InitialiseRepository();

            // act
            var result = await _target.Handle(new() { Id = entity1.Id });

            // assert
            Assert.NotNull(result);
            Assert.NotNull(result.Item);
            Assert.True(result.Items?.Count == 1);
            Assert.Equal(result.Item!.Uuid, entity1.Uuid);
        }

        private async Task<(EntityStub, EntityStub)> InitialiseRepository()
        {
            var entity1 = await _repository.Add(new());
            var entity2 = await _repository.Add(new());
            await _repository.Save();
            return (entity1, entity2);
        }
    }
}
