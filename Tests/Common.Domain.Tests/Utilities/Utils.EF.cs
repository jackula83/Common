using Common.Domain.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Domain.Tests.Utilities
{
    public static partial class Utils
    {
        public static TContext? CreateInMemoryDatabase<TContext>(string databaseName, bool shared = false)
            where TContext : FxDbContext
        {
            // use a service provider to make the in-memory database unique for each fact/theory
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(databaseName);

            if (!shared)
                builder = builder.UseInternalServiceProvider(serviceProvider);

            var options = builder
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return Activator.CreateInstance(typeof(TContext), options) as TContext;
        }
    }
}
