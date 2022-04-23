namespace Common.Domain.Core.Requests
{
    public abstract class FxEntityQueryRequest : FxQueryRequest
    {
        public int Id { get; set; }
    }
}
