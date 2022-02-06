using Common.Domain.Core.Extensions;
using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Data
{
    public abstract class FxEntityRepository<TEntityType, TContextType> : IFxEntityRepository<TEntityType>
       where TEntityType : FxEntity
       where TContextType : FxDbContext
    {
        protected readonly TContextType _context;

        public FxEntityRepository(TContextType context)
        {
            _context = context;
        }

        public virtual async Task<int> Add(TEntityType entity)
        {
            await _context.AddAsync(entity);
            return entity.Id;
        }

        public virtual async Task<List<TEntityType>> Enumerate(bool includeDeleted)
        {
            var models = await _context.Set<TEntityType>().ToListAsync();
            var foundModels = models.FindAll(x => (includeDeleted || !x.DeleteFlag)).ToList();
            return foundModels;
        }

        public virtual async Task<TEntityType?> Get(int id)
            => await _context.Set<TEntityType>().FindAsync(id);

        public virtual async Task<bool> Update(TEntityType entity)
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

        public virtual async Task<bool> Delete(TEntityType entity)
        {
            if (entity == default)
                return false;

            return await this.Delete(entity.Id);
        }
    }
}
