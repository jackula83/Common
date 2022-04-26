using Common.Domain.Core.Data;

namespace Common.Domain.Core.Requests
{
    public abstract class FxEntityCommandRequest<TEntity> : FxCommandRequest
        where TEntity : FxEntity
    {
        public TEntity? Item { get; set; }
    }
}
