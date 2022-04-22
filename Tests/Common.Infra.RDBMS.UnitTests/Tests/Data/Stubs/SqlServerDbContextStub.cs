using Common.Domain.UnitTests.Models.Stubs;
using Common.Infra.RDBMS.Data;
using Microsoft.EntityFrameworkCore;

namespace Common.Infra.RDBMS.UnitTests.Tests.Data.Stubs
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
