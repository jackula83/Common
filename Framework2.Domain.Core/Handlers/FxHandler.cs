using Framework2.Domain.Core.Requests;
using Framework2.Domain.Core.Responses;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Core.Handlers
{
    public abstract class FxHandler
    {
    }

    public abstract class FxHandler<TRequest> : FxHandler
        where TRequest : FxRequest
    {
        protected readonly ILogger<FxHandler<TRequest>> _logger;

        public FxHandler(ILogger<FxHandler<TRequest>> logger)
        {
            _logger = logger;
        }

        protected abstract Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);

        public virtual async Task Handle(TRequest request, CancellationToken cancellationToken = default)
            => await ExecuteAsync(request, cancellationToken);
    }

    public abstract class FxHandler<TRequest, TResponse> : FxHandler
        where TRequest : FxRequest
        where TResponse : FxResponse
    {
        protected readonly ILogger<FxHandler<TRequest, TResponse>> _logger;

        public FxHandler(ILogger<FxHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
            => await ExecuteAsync(request, cancellationToken);
    }
}
