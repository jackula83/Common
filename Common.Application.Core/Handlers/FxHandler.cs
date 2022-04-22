using Common.Domain.Core.Models;

namespace Common.Application.Core.Handlers
{
    public abstract class FxHandler
    {
    }

    public abstract class FxHandler<TRequest> : FxHandler
        where TRequest : FxRequest
    {
        protected abstract Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);

        public virtual async Task Handle(TRequest request, CancellationToken cancellationToken = default)
            => await this.ExecuteAsync(request, cancellationToken);
    }

    public abstract class FxHandler<TRequest, TResponse> : FxHandler
        where TRequest : FxRequest
        where TResponse : FxResponse
    {
        protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
            => await this.ExecuteAsync(request, cancellationToken);
    }
}
