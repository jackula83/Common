using Common.Domain.Core.Models;

namespace Common.Domain.Core.Responses
{
    public abstract class FxEntityCommandResponse : FxCommandResponse
    {
        public FxEntity? Item { get; set; }
        public bool Success { get; set; } = true;
    }
}
