using Common.Domain.Core.Data;
using Common.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Domain.Core.Extensions
{
    public static partial class Extensions
    {
        public static void DetachLocal<TEntityType>(this FxDbContext context, TEntityType model, int modelId)
           where TEntityType : FxEntity
        {
            var localModel = context.Set<TEntityType>()
               .Local
               .FirstOrDefault(x => x.Id == modelId);

            if (localModel != default)
                context.Entry(localModel).State = EntityState.Detached;

            context.Entry(model).State = EntityState.Modified;
        }
    }
}
