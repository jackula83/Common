using Common.Application.Core.Models;
using Microsoft.Extensions.Logging;

namespace Common.Application.Core.Handlers
{
    public abstract class FxCommandHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
        where TRequest : FxCommandRequest
        where TResponse : FxCommandResponse, new()
    {
        public FxCommandHandler(ILogger<FxCommandHandler<TRequest, TResponse>> logger)
            : base(logger)
        {
        }
    }
}
