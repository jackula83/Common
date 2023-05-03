using Framework2.Infra.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Framework2.Infra.Data.Context
{
    public abstract class FxDbContext : DbContext
    {
        private readonly IMediator _mediator;

        protected abstract void Setup<TEntity>(ModelBuilder builder) where TEntity : FxDataObject;

        public FxDbContext(DbContextOptions options, IMediator mediator) : base(options) 
        {
            _mediator = mediator;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var (aggregateRoots, domainEvents) = GetAggregateRootsAndDomainEvents();
            DispatchDomainEvents(domainEvents);
            ClearDomainEvents(aggregateRoots);

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var propertyTypes = GetType().GetProperties()
               .Select(p => p.PropertyType);

            var entityTypes = propertyTypes
               ?.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(DbSet<>))
               ?.Select(p => p.GetGenericArguments().FirstOrDefault());

            foreach (var entityType in entityTypes ?? Enumerable.Empty<Type>())
            {
                var setupMethod = GetType()
                    !.GetMethod(nameof(Setup), BindingFlags.Instance | BindingFlags.NonPublic)
                    !.MakeGenericMethod(new Type[] { entityType! })
                    !.Invoke(this, new object[] { modelBuilder });
            }
        }

        private (List<FxAggregateRoot>, List<FxDomainEvent>) GetAggregateRootsAndDomainEvents()
        {
            var aggregateRoots = this.ChangeTracker
                .Entries<FxAggregateRoot>()
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = aggregateRoots
                .SelectMany(x => x.DomainEvents)
                .ToList();

            return (aggregateRoots, domainEvents);
        }

        private void DispatchDomainEvents(List<FxDomainEvent> domainEvents)
            => domainEvents.ForEach(e => _mediator.Publish(e, default));

        private void ClearDomainEvents(List<FxAggregateRoot> aggregateRoots)
            => aggregateRoots.ForEach(r => r.ClearDomainEvents());
    }
}
