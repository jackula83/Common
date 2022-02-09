using Common.Domain.Core.Data;
using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Domain.Core.Extensions
{
    public static partial class Extensions
    {
        public static void DetachLocal<TEntity>(this FxDbContext context, int entityId, TEntity entity)
           where TEntity : FxEntity
        {
            var localEntity = context.Set<TEntity>()
               .Local
               .FirstOrDefault(x => x.Id == entityId);

            if (localEntity != default)
                context.Entry(localEntity).State = EntityState.Detached;

            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
