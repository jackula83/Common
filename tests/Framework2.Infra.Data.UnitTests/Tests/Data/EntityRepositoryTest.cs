using Framework2.Infra.Data.UnitTests.Tests.Data.Stubs;
using Framework2.Infra.Data.UnitTests.Tests.Models.Stubs;
using Framework2.Infra.Data.UnitTests.Utilities;
using MediatR;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Framework2.Infra.Data.UnitTests.Tests.Data
{
    public class EntityRepositoryTest
    {
        private readonly EntityRepositoryStub _repository;
        private readonly SqlServerDbContextStub _context;
        private readonly Mock<IMediator> _mediatorMock;

        public EntityRepositoryTest()
        {
            _mediatorMock = new();
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(EntityRepositoryTest), _mediatorMock.Object)!;
            _repository = new(_context);
        }

        [Fact]
        public async Task Add_AddEntity_HasNoSideEffectOnArgument()
        {
            // arrange
            var entity = new EntityStub()
            {
                Id = 0,
                Uuid = Guid.NewGuid(),
                CreatedBy = "a",
                UpdatedBy = "a",
                CreatedAt = DateTime.Now.AddDays(-1),
                UpdatedAt = DateTime.Now.AddDays(-1),
            };
            var serialExpected = JsonConvert.SerializeObject(entity);

            // act
            var savedEntity = await _repository.Add(entity);
            var serialActual = JsonConvert.SerializeObject(entity);

            // assert
            Assert.Equal(1, savedEntity.Id);
            Assert.Equal(serialExpected, serialActual);
        }

        [Fact]
        public async Task Add_AddEntity_AutoGeneratesValues()
        {
            // arrange
            var entity = new EntityStub();

            // act
            var savedEntity = await _repository.Add(entity);
            var dbEntity = await _repository.Get(savedEntity.Id);

            // assert
            Assert.NotNull(dbEntity);
            Assert.Equal(1, savedEntity.Id);
            Assert.Equal(1, dbEntity!.Id);
            Assert.NotEqual(entity.Uuid, dbEntity!.Uuid);
            Assert.NotEqual(entity.CreatedAt, dbEntity!.CreatedAt);
        }

        [Theory]
        [MemberData(nameof(EnumerateEntitiesProvider))]
        public async Task Enumerate_EntitiesAdded_ReturnsEntities(List<EntityStub> entities, int expectedCount)
        {
            // arrange
            entities.ForEach(async x => await _repository.Add(x));
            await _context.SaveChangesAsync();

            // act
            var result = await _repository.Enumerate();

            // assert
            Assert.Equal(expectedCount, result.Count);
        }

        public static IEnumerable<object[]> EnumerateEntitiesProvider()
        {
            yield return new object[]
            {
                new List<EntityStub>()
                {
                    new() {DeleteFlag = true}
                },
                0
            };

            yield return new object[]
            {
                new List<EntityStub>()
                {
                    new()
                },
                1
            };

            yield return new object[]
            {
                new List<EntityStub>()
                {
                    new() {DeleteFlag = true},
                    new()
                },
                1
            };
        }
    }
}
