using Framework2.Infra.Data.Entity;

namespace Framework2.Domain.Core.Responses
{
    public abstract class FxEntityQueryResponse : FxQueryResponse { }

    public abstract class FxEntityQueryResponse<TDataObject> : FxEntityQueryResponse
        where TDataObject : class, IDataObject
    {
        public List<TDataObject> Items { get; set; } = new();

        public TDataObject? Item => Items?.FirstOrDefault();
    }
}
