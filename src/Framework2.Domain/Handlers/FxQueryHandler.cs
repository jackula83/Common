using Framework2.Domain.Core.Requests;
using Framework2.Domain.Core.Responses;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Core.Handlers
{
    public abstract class FxQueryHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
        where TRequest : FxQueryRequest
        where TResponse : FxQueryResponse, new()
    {
        protected FxQueryHandler(ILogger<FxQueryHandler<TRequest, TResponse>> logger) : base(logger)
        {
        }
    }
}
