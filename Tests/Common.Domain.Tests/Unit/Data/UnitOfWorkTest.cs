using Common.Domain.Tests.Unit.Data.Stubs;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.Tests.Utilities;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Common.Domain.Tests.Unit.Data
{
    public class UnitOfWorkStubTest
    {
        private readonly UnitOfWorkStub _instance;
        private readonly SqlServerDbContextStub _context;

        public UnitOfWorkStubTest()
        {
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(UnitOfWorkStubTest))!;
            _instance = new(_context);
        }

        [Fact]
        public async Task Save_AddNewEntities_EntityAddedToDatabase()
        {
            // arrange
            var entity1 = await _instance.Repository.Add(new EntityStub());
            var entity2 = await _instance.Repository.Add(new EntityStub());

            // act
            var dbEntity1 = await _instance.Repository.Get(entity1.Id);
            var dbEntity2 = await _instance.Repository.Get(entity2.Id);
            await _instance.Save();

            // assert
            Assert.Equal(1, entity1.Id);
            Assert.Equal(2, entity2.Id);
            Assert.NotNull(dbEntity1);
            Assert.NotNull(dbEntity2);
            Assert.Equal(1, dbEntity1!.Id);
            Assert.Equal(2, dbEntity2!.Id);
            Assert.False(_context.ChangeTracker.HasChanges());
        }

        [Fact]
        public async Task Save_DeleteEntity_EntityNotEnumeratedUnlessSpecified()
        {
            // arrange
            var id = await _instance.Repository.Add(new EntityStub());

            // act
            await _instance.Save();
            await _instance.Repository.Delete(id);
            var entitiesIncDeleted = await _instance.Repository.Enumerate(true);
            var entitiesExDeleted = await _instance.Repository.Enumerate(false);

            // assert
            Assert.NotEmpty(entitiesIncDeleted);
            Assert.Empty(entitiesExDeleted);
            Assert.True(_context.ChangeTracker.HasChanges());
        }

        [Fact]
        public async Task Save_DeleteEntity_GetsDeletedEntity()
        {
            // arrange
            var entity = await _instance.Repository.Add(new EntityStub());

            // act
            await _instance.Save();
            await _instance.Repository.Delete(entity.Id);
            var deletedEntity = await _instance.Repository.Get(entity.Id);

            // assert
            Assert.NotNull(deletedEntity);
            Assert.True(deletedEntity!.DeleteFlag);
            Assert.True(_context.ChangeTracker.HasChanges());
        }

        [Fact]
        public async Task Save_UpdateEntity_ConsistentCreatedAndUpdatedTimestamps()
        {
            // arrange
            var entity = await _instance.Repository.Add(new EntityStub());
            // unfortunately SQL server works in 3ms per tick on default datetime, so we need to intentionally pause after the update
            Thread.Sleep(3);

            // act
            var dbEntityBeforeUpdate = await _instance.Repository.Get(entity.Id);
            await _instance.Save();
            await _instance.Repository.Update(dbEntityBeforeUpdate!);
            var dbEntityAfterUpdate = await _instance.Repository.Get(entity.Id);

            // assert
            Assert.NotNull(dbEntityBeforeUpdate);
            Assert.NotNull(dbEntityAfterUpdate);
            Assert.Null(dbEntityBeforeUpdate!.UpdatedAt);
            Assert.NotNull(dbEntityAfterUpdate!.UpdatedAt);
            Assert.True(dbEntityAfterUpdate.UpdatedAt > dbEntityAfterUpdate.CreatedAt);
            Assert.Equal(dbEntityBeforeUpdate.CreatedAt, dbEntityAfterUpdate.CreatedAt);
            Assert.True(_context.ChangeTracker.HasChanges());
        }
    }
}
