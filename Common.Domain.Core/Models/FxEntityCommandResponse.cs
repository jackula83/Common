namespace Common.Domain.Core.Models
{
    public abstract class FxEntityCommandResponse : FxCommandResponse
    {
        public FxEntity? Item { get; set; }
        public bool Success { get; set; } = true;
    }
}
