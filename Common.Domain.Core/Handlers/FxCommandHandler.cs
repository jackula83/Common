using Common.Domain.Core.Requests;
using Common.Domain.Core.Responses;
using Microsoft.Extensions.Logging;

namespace Common.Domain.Core.Handlers;

public abstract class FxCommandHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
    where TRequest : FxCommandRequest
    where TResponse : FxCommandResponse, new()
{
    public FxCommandHandler(ILogger<FxCommandHandler<TRequest, TResponse>> logger)
        : base(logger)
    {
    }
}
