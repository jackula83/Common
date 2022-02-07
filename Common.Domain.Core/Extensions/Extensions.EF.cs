using Common.Domain.Core.Data;
using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Domain.Core.Extensions
{
    public static partial class Extensions
    {
        public static void DetachLocal<TEntity>(this FxDbContext context, TEntity model, int modelId)
           where TEntity : FxEntity
        {
            var localModel = context.Set<TEntity>()
               .Local
               .FirstOrDefault(x => x.Id == modelId);

            if (localModel != default)
                context.Entry(localModel).State = EntityState.Detached;

            context.Entry(model).State = EntityState.Modified;
        }
    }
}
