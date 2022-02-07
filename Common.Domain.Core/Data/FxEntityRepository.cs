using Common.Domain.Core.Extensions;
using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Domain.Core.Data
{
    public abstract class FxEntityRepository<TContext, TEntity> : IFxEntityRepository<TEntity>
       where TContext : FxDbContext
       where TEntity : FxEntity
    {
        protected readonly TContext _context;

        public FxEntityRepository(TContext context)
        {
            _context = context;
        }

        public virtual async Task<int> Add(TEntity entity)
        {
            var copy = entity.Copy();
            await _context.AddAsync(copy
                .Tap(x => x.Uuid = Guid.NewGuid())
                .Tap(x => x.CreatedAt = DateTime.UtcNow));
            return copy.Id;
        }

        public virtual async Task<List<TEntity>> Enumerate(bool includeDeleted = false)
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            var foundEntities = entities.FindAll(x => (includeDeleted || !x.DeleteFlag)).ToList();
            return foundEntities;
        }

        public virtual async Task<TEntity?> Get(int id)
            => await _context.Set<TEntity>().FindAsync(id);

        public virtual async Task<bool> Update(TEntity entity)
        {
            var m = await this.Get(entity.Id);
            if (m == default)
                return false;

            var copy = entity.Copy();
            _context.DetachLocal(
                entity.Id,
                copy
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));

            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await this.Get(id);
            if (entity == default)
                return false;

            return await this.Update(entity
                .Tap(x => x.DeleteFlag = true)
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            if (entity == default)
                return false;

            return await this.Delete(entity.Id);
        }
    }
}
