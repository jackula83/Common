using Framework2.Infra.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Framework2.Infra.Data.UnitTests.Tests.Context.Stubs
{
    public class DbContextStub : FxDbContext
    {
        public DbSet<AggregateRootStub> Children { get; set; }

#pragma warning disable 8618
        public DbContextStub(DbContextOptions options, IMediator mediator) : base(options, mediator)
        {
        }
#pragma warning restore 8618

        protected override void Setup<TEntity>(ModelBuilder builder)
        {
        }
    }
}
