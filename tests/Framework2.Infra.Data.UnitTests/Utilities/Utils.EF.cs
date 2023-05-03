using Framework2.Infra.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework2.Infra.Data.UnitTests.Utilities
{
    public static partial class Utils
    {
        public static TContext? CreateInMemoryDatabase<TContext>(string databaseName, IMediator mediator, bool shared = false)
            where TContext : FxDbContext
        {
            // use a service provider to make the in-memory database unique for each fact/theory
            var serviceProvider = CreateServiceProvider();
            var builder = CreateInMeoryOptionsBuilder<TContext>(databaseName);

            if (!shared)
                builder = builder.UseInternalServiceProvider(serviceProvider);

            var options = builder
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return Activator.CreateInstance(typeof(TContext), options, mediator) as TContext;
        }

        public static DbContextOptions CreateInMemoryDatabaseOptions<TContext>(string databaseName)
            where TContext : FxDbContext
        {
            var serviceProvider = CreateServiceProvider();
            var builder = CreateInMeoryOptionsBuilder<TContext>(databaseName);
            return builder.Options;
        }

        private static IServiceProvider CreateServiceProvider()
            => new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

        private static DbContextOptionsBuilder<TContext> CreateInMeoryOptionsBuilder<TContext>(string databaseName)
            where TContext : FxDbContext
          => new DbContextOptionsBuilder<TContext>()
                .UseInMemoryDatabase(databaseName);
    }
}
