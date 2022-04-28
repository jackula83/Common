using Framework2.Domain.Core.Data;

namespace Framework2.Domain.Core.Requests
{
    public abstract class FxEntityCommandRequest<TEntity> : FxCommandRequest
        where TEntity : FxEntity
    {
        public TEntity? Item { get; set; }
    }
}
