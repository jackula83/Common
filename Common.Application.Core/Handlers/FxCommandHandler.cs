using Common.Domain.Core.Models;

namespace Common.Application.Core.Handlers
{
    public abstract class FxCommandHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
        where TRequest : FxCommandRequest
        where TResponse : FxCommandResponse, new()
    {
    }
}
