using Common.Domain.Core.Extensions;
using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Domain.Core.Data
{
    public abstract class FxEntityRepository<TContext, TEntity> : IEntityRepository<TEntity>
       where TContext : FxDbContext
       where TEntity : FxEntity
    {
        protected readonly TContext _context;

        public FxEntityRepository(TContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var copy = entity.Copy();
            await _context.AddAsync(copy
                .Tap(x => x.Uuid = Guid.NewGuid())
                .Tap(x => x.CreatedAt = DateTime.UtcNow));
            return copy;
        }

        public virtual async Task<List<TEntity>> Enumerate(bool includeDeleted = false)
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            var foundEntities = entities.FindAll(x => (includeDeleted || !x.DeleteFlag)).ToList();
            return foundEntities;
        }

        public virtual async Task<TEntity?> Get(int id)
            => await _context.Set<TEntity>().FindAsync(id);

        public virtual async Task<TEntity?> Update(TEntity entity)
        {
            var m = await this.Get(entity.Id);
            if (m == default)
                return default;

            var copy = entity.Copy();
            _context.DetachLocal(
                entity.Id,
                copy
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));

            return copy;
        }

        public virtual async Task<TEntity?> Delete(int id)
        {
            var entity = await this.Get(id);
            if (entity == default)
                return default;

            return await this.Update(entity
                .Tap(x => x.DeleteFlag = true)
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));
        }

        public virtual async Task<TEntity?> Delete(TEntity entity)
        {
            if (entity == default)
                return default;

            return await this.Delete(entity.Id);
        }

        public virtual async Task Save()
            => await _context.SaveChangesAsync();
    }
}
