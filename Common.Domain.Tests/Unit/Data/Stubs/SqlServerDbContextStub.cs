using Common.Domain.Core.Data;
using Common.Domain.Tests.Unit.Models.Stubs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Tests.Unit.Data.Stubs
{
    internal class SqlServerDbContextStub : FxSqlServerDbContext
    {
        public DbSet<EntityStub> Entities { get; set; }

        public SqlServerDbContextStub(DbContextOptions options) : base(options)
        {
        }
    }
}
