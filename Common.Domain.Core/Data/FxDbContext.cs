using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Common.Domain.Core.Data
{
    public abstract class FxDbContext : DbContext
    {
        protected abstract void Setup<TEntity>(ModelBuilder builder) where TEntity : FxEntity;

        public FxDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var propertyTypes = this.GetType().GetProperties()
               .Select(p => p.PropertyType);

            var modelTypes = propertyTypes
               ?.Where(x => x.GetGenericTypeDefinition() == typeof(DbSet<>))
               ?.Select(p => p.GetGenericArguments().FirstOrDefault());

            foreach (var modelType in modelTypes ?? Enumerable.Empty<Type>())
            {
                var setupMethod = this.GetType()
                    !.GetMethod(nameof(Setup))
                    !.MakeGenericMethod(this.GetType())
                    !.Invoke(modelType, new object[] { modelBuilder });
            }
        }
    }
}
