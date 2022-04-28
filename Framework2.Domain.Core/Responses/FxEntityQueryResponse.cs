using Framework2.Domain.Core.Data;

namespace Framework2.Domain.Core.Responses
{
    public abstract class FxEntityQueryResponse : FxQueryResponse { }

    public abstract class FxEntityQueryResponse<TEntity> : FxEntityQueryResponse
        where TEntity : FxEntity
    {
        public List<TEntity> Items { get; set; } = new();

        public TEntity? Item => Items?.FirstOrDefault();
    }
}
