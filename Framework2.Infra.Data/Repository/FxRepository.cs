using Framework2.Core.Extensions;
using Framework2.Infra.Data.Context;
using Framework2.Infra.Data.Entity;
using Framework2.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Framework2.Infra.Data.Repository
{
    public abstract class FxRepository<TContext, TAggregateRoot> : IEntityRepository<TAggregateRoot>
       where TContext : FxDbContext
       where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly TContext _context;

        public FxRepository(TContext context)
        {
            _context = context;
        }

        public virtual async Task<TAggregateRoot> Add(TAggregateRoot entity)
        {
            var copy = entity.Copy();
            await _context.AddAsync(copy
                .Tap(x => x.Uuid = Guid.NewGuid())
                .Tap(x => x.CreatedAt = DateTime.UtcNow));
            return copy;
        }

        public virtual async Task<List<TAggregateRoot>> Enumerate(bool includeDeleted = false)
        {
            var entities = await _context.Set<TAggregateRoot>().ToListAsync();
            var foundEntities = entities.FindAll(x => includeDeleted || !x.DeleteFlag).ToList();
            return foundEntities;
        }

        public virtual async Task<TAggregateRoot?> Get(int id)
            => await _context.Set<TAggregateRoot>().FindAsync(id);

        public virtual async Task<TAggregateRoot?> Update(TAggregateRoot entity)
        {
            var m = await Get(entity.Id);
            if (m == default)
                return default;

            var copy = entity.Copy();
            _context.DetachLocal(
                entity.Id,
                copy
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));

            return copy;
        }

        public virtual async Task<TAggregateRoot?> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == default)
                return default;

            return await Update(entity
                .Tap(x => x.DeleteFlag = true)
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));
        }

        public virtual async Task<TAggregateRoot?> Delete(TAggregateRoot entity)
        {
            if (entity == default)
                return default;

            return await Delete(entity.Id);
        }

        public virtual async Task Save()
            => await _context.SaveChangesAsync();
    }
}
