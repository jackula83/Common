using Framework2.Core.Extensions;
using Framework2.Infra.Data.Context;
using Framework2.Infra.Data.Entity;
using Framework2.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Framework2.Infra.Data.Repository
{
    public abstract class FxRepository<TContext, TDataObject> : IEntityRepository<TDataObject>
       where TContext : FxDbContext
       where TDataObject : class, IDataObject
    {
        protected readonly TContext _context;

        public FxRepository(TContext context)
        {
            _context = context;
        }

        public virtual async Task<TDataObject> Add(TDataObject entity)
        {
            var copy = entity.Copy();
            await _context.AddAsync(copy
                .Tap(x => x.Uuid = Guid.NewGuid())
                .Tap(x => x.CreatedAt = DateTime.UtcNow));
            return copy;
        }

        public virtual async Task<List<TDataObject>> Enumerate(bool includeDeleted = false)
        {
            var entities = await _context.Set<TDataObject>().ToListAsync();
            var foundEntities = entities.FindAll(x => includeDeleted || !x.DeleteFlag).ToList();
            return foundEntities;
        }

        public virtual async Task<TDataObject?> Get(int id)
            => await _context.Set<TDataObject>().FindAsync(id);

        public virtual async Task<TDataObject?> Update(TDataObject entity)
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

        public virtual async Task<TDataObject?> Delete(int id)
        {
            var entity = await Get(id);
            if (entity == default)
                return default;

            return await Update(entity
                .Tap(x => x.DeleteFlag = true)
                .Tap(x => x.UpdatedAt = DateTime.UtcNow));
        }

        public virtual async Task<TDataObject?> Delete(TDataObject entity)
        {
            if (entity == default)
                return default;

            return await Delete(entity.Id);
        }

        public virtual async Task Save()
            => await _context.SaveChangesAsync();
    }
}
