using Framework2.Infra.Data.Entity;

namespace Framework2.Domain.Core.Requests
{
    public abstract class FxEntityCommandRequest<TDataObject> : FxCommandRequest
        where TDataObject : IDataObject
    {
        public TDataObject? Item { get; set; }
    }
}
