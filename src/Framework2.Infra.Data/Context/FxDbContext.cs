using Framework2.Infra.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Framework2.Infra.Data.Context
{
    public abstract class FxDbContext : DbContext
    {
        protected abstract void Setup<TEntity>(ModelBuilder builder) where TEntity : FxDataObject;

        public FxDbContext(DbContextOptions options) : base(options) { }

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
    }
}
