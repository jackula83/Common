using Framework2.Domain.Core.Requests;
using Framework2.Domain.Core.Responses;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Core.Handlers;

public abstract class FxCommandHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
    where TRequest : FxCommandRequest
    where TResponse : FxCommandResponse, new()
{
    public FxCommandHandler(ILogger<FxCommandHandler<TRequest, TResponse>> logger)
        : base(logger)
    {
    }
}
