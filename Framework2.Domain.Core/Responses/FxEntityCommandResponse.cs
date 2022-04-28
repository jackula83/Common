using Framework2.Domain.Core.Data;

namespace Framework2.Domain.Core.Responses
{
    public abstract class FxEntityCommandResponse : FxCommandResponse
    {
        public FxEntity? Item { get; set; }
        public bool Success { get; set; } = true;
    }
}
