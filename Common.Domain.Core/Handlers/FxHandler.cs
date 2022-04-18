using Common.Domain.Core.Models;

namespace Common.Domain.Core.Handlers
{
    public abstract class FxHandler
    {
        protected abstract Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : FxRequest;
    }

    public abstract class FxHandler<TRequest> : FxHandler
        where TRequest : FxRequest
    {
        public virtual async Task Handle(TRequest request, CancellationToken cancellationToken = default)
            => await this.ExecuteAsync<TRequest, bool>(request, cancellationToken);
    }

    public abstract class FxHandler<TRequest, TResponse> : FxHandler
        where TRequest : FxRequest
        where TResponse : FxResponse
    {
        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
            => await this.ExecuteAsync<TRequest, TResponse>(request, cancellationToken);
    }
}
