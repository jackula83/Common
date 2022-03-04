namespace Common.Domain.Core.Models
{
    public abstract class FxRequest
    {
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
    }
}
