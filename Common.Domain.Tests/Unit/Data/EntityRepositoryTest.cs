using Common.Domain.Tests.Unit.Data.Stubs;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.Tests.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Common.Domain.Tests.Unit.Data
{
    public class EntityRepositoryTest
    {
        private readonly EntityRepositoryStub _repository;
        private readonly SqlServerDbContextStub _context;

        public EntityRepositoryTest()
        {
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(EntityRepositoryTest))!;
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
            var id = await _repository.Add(entity);
            var serialActual = JsonConvert.SerializeObject(entity);

            // assert
            Assert.Equal(1, id);
            Assert.Equal(serialExpected, serialActual);
        }

        [Fact]
        public async Task Add_AddEntity_AutoGeneratesValues()
        {
            // arrange
            var entity = new EntityStub();

            // act
            var id = await _repository.Add(entity);
            var dbEntity = await _repository.Get(id);

            // assert
            Assert.NotNull(dbEntity);
            Assert.Equal(1, id);
            Assert.Equal(1, dbEntity!.Id);
            Assert.NotEqual(entity.Uuid, dbEntity!.Uuid);
            Assert.NotEqual(entity.CreatedAt, dbEntity!.CreatedAt);
        }

        [Theory]
        [MemberData(nameof(EnumerateEntitiesProvider))]
        internal async Task Enumerate_EntitiesAdded_ReturnsEntities(List<EntityStub> entities, int expectedCount)
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
