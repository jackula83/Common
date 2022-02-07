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
            await _context.AddAsync(entity);
            return entity.Id;
        }

        public virtual async Task<List<TEntity>> Enumerate(bool includeDeleted = false)
        {
            var models = await _context.Set<TEntity>().ToListAsync();
            var foundModels = models.FindAll(x => (includeDeleted || !x.DeleteFlag)).ToList();
            return foundModels;
        }

        public virtual async Task<TEntity?> Get(int id)
            => await _context.Set<TEntity>().FindAsync(id);

        public virtual async Task<bool> Update(TEntity entity)
        {
            var m = await this.Get(entity.Id);
            if (m == default)
                return false;

            _context.DetachLocal(entity, entity.Id);

            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var model = await this.Get(id);
            if (model == default)
                return false;

            return await this.Update(model.Tap(x => x.DeleteFlag = true));
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            if (entity == default)
                return false;

            return await this.Delete(entity.Id);
        }
    }
}
