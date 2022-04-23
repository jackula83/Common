using Common.Domain.Core.Models;

namespace Common.Domain.Core.Requests
{
    public abstract class FxEntityCommandRequest<TEntity> : FxCommandRequest
        where TEntity : FxEntity
    {
        public TEntity? Item { get; set; }
    }
}
