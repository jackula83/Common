using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Data
{
    /// <summary>
    /// DbContext that will work with SQL Server only
    /// </summary>
    public abstract class FxSqlServerDbContext : FxDbContext
    {
        protected FxSqlServerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void Setup<TEntityType>(ModelBuilder builder)
        {
            builder.Entity<TEntityType>(x => 
            {
                x.Property(p => p.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();

                x.Property(p => p.Uuid)
                    .HasDefaultValueSql("NEWID()")
                    .ValueGeneratedOnAdd();

                x.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnAdd();

                x.Property(p => p.UpdatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .ValueGeneratedOnUpdate();
            });
        }
    }
}
