using Common.Domain.UnitTests.Tests.Models.Stubs;
using Common.Infra.RDBMS;
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
