using Common.Domain.Core.Models;

namespace Common.Domain.Core.Handlers
{
    public abstract class FxCommandHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
        where TRequest : FxCommandRequest
        where TResponse : FxCommandResponse, new()
    {
    }
}
