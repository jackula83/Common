using Common.Domain.Core.Data;
using Common.Domain.Core.Models;
using Common.Domain.Tests.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.Domain.Tests.Fx
{
    public abstract class FxEntityRepositoryTest<TContext, TEntity>
        where TContext : FxDbContext
        where TEntity : FxEntity, new()
    {
        protected readonly FxEntityRepository<TContext, TEntity> _repository;

        public FxEntityRepositoryTest(FxEntityRepository<TContext, TEntity> repository)
        {
            _repository = repository;
        }

        [Fact]
        public async Task Add_AddEntity_HasNoSideEffectOnArgument()
        {
            // arrange
            var entity = new TEntity()
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
            var entity = new TEntity();

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
    }
}
