using Common.Domain.Tests.Unit.Data.Stubs;
using Common.Domain.Tests.Unit.Models.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
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
            var _contextOptions = new DbContextOptionsBuilder<SqlServerDbContextStub>()
                .UseInMemoryDatabase(nameof(UnitOfWorkStub))
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new SqlServerDbContextStub(_contextOptions);
            _instance = new(_context);
        }

        [Fact]
        public async Task Save_AddNewEntities_EntityAddedToDatabase()
        {
            // act
            var id1 = await _instance.Repository.Add(new EntityStub());
            var id2 = await _instance.Repository.Add(new EntityStub());
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
    }
}
