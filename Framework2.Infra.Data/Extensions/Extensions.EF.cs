using Framework2.Infra.Data.Context;
using Framework2.Infra.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Framework2.Infra.Data.Extensions
{
    public static partial class Extensions
    {
        public static void DetachLocal<TDataObject>(this FxDbContext context, int entityId, TDataObject dataObject)
           where TDataObject : class, IDataObject
        {
            var localEntity = context.Set<TDataObject>()
               .Local
               .FirstOrDefault(x => x.Id == entityId);

            if (localEntity != default)
                context.Entry(localEntity).State = EntityState.Detached;

            context.Entry(dataObject).State = EntityState.Modified;
        }
    }
}
