using Common.Domain.Core.Requests;
using Common.Domain.Core.Responses;
using Microsoft.Extensions.Logging;

namespace Common.Domain.Core.Handlers
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
