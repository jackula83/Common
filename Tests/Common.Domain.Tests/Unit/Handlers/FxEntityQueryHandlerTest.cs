using Common.Domain.Tests.Unit.Data.Stubs;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.Tests.Utilities;
using Common.Domain.UnitTests.Unit.Handlers.Stubs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Common.Domain.UnitTests.Unit.Handlers
{
    public class FxEntityQueryHandlerTest
    {
        private readonly EntityQueryHandlerStub _target;
        private readonly SqlServerDbContextStub _context;
        private readonly EntityRepositoryStub _repository;

        public FxEntityQueryHandlerTest()
        {
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(FxEntityQueryHandlerTest))!;
            _repository = new(_context);
            _target = new(_repository);
        }

        [Fact]
        public async Task Handle_DefaultRequestSent_ReturnsEntityCollection()
        {
            // arrange
            var entity1 = await _repository.Add(new());
            var entity2 = await _repository.Add(new());
            await _repository.Save();

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
            await Task.CompletedTask;
        }
    }
}
