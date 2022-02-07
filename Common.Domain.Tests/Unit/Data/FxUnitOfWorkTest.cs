using Common.Domain.Tests.Unit.Data.Stubs;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.Tests.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Common.Domain.Tests.Unit.Data
{
    public class UnitOfWorkStubTest
    {
        private UnitOfWorkStub _instance;
        private SqlServerDbContextStub _context;

        public UnitOfWorkStubTest()
        {
            _context = Utils.CreateInMemoryDatabase<SqlServerDbContextStub>(nameof(UnitOfWorkStubTest))!;
            _instance = new(_context);
        }

        [Fact]
        public async Task Save_AddNewEntities_EntityAddedToDatabase()
        {
            // arrange
            var id1 = await _instance.Repository.Add(new EntityStub());
            var id2 = await _instance.Repository.Add(new EntityStub());

            // act
            var dbEntity1 = await _instance.Repository.Get(id1);
            var dbEntity2 = await _instance.Repository.Get(id2);
            await _instance.Save();

            // assert
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
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
            var entity = new EntityStub();
            var id = await _instance.Repository.Add(entity);

            // act
            await _instance.Save();
            await _instance.Repository.Delete(id);
            var entitiesIncDeleted = await _instance.Repository.Enumerate(true);
            var entitiesExDeleted = await _instance.Repository.Enumerate();

            // assert
            Assert.NotEmpty(entitiesIncDeleted);
            Assert.Empty(entitiesExDeleted);
        }
    }
}
