namespace Framework2.Domain.Core.Requests
{
    public abstract class FxRequest
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
    }
}
