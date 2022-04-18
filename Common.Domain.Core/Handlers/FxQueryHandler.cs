using Common.Domain.Core.Models;

namespace Common.Domain.Core.Handlers
{
    public abstract class FxQueryHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
        where TRequest : FxQueryRequest
        where TResponse : FxQueryResponse, new()
    {
    }
}
