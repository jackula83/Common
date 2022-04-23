using Common.Domain.Core.Data;
using Common.Domain.UnitTests.Models.Stubs;
using Microsoft.EntityFrameworkCore;

namespace Common.Domain.UnitTests.Tests.Data.Stubs
{
    public class SqlServerDbContextStub : FxSqlServerDbContext
    {
        public DbSet<EntityStub> Entities { get; set; }

#pragma warning disable 8618
        public SqlServerDbContextStub(DbContextOptions options) : base(options)
        {
        }
#pragma warning restore 8618
    }
}
