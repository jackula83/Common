namespace Common.Domain.Core.Models
{
    public abstract class FxEntityCommandResponse : FxCommandResponse
    {
        public int? Id { get; set; }
        public bool Success { get; set; } = true;
    }
}
