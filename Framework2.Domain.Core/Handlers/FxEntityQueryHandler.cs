using Framework2.Domain.Core.Data;
using Framework2.Domain.Core.Requests;
using Framework2.Domain.Core.Responses;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Core.Handlers
{
    public abstract class FxEntityQueryHandler<TRequest, TResponse, TEntity> : FxQueryHandler<TRequest, TResponse>
        where TRequest : FxEntityQueryRequest
        where TResponse : FxEntityQueryResponse<TEntity>, new()
        where TEntity : FxEntity
    {
        protected readonly IEntityRepository<TEntity> _repository;

        public FxEntityQueryHandler(
            ILogger<FxEntityQueryHandler<TRequest, TResponse, TEntity>> logger,
            IEntityRepository<TEntity> repository)
            : base(logger)
            => _repository = repository;

        protected override async Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            var result = new TResponse();

            if (request.Id == 0)
                result.Items = await GetCollection();
            else
            {
                var model = await GetSingle(request.Id);
                if (model != null)
                    result.Items.Add(model);
            }

            return result;
        }

        protected virtual async Task<TEntity?> GetSingle(int id)
            => await _repository.Get(id);

        protected virtual async Task<List<TEntity>> GetCollection(bool includeDeleted = false)
            => await _repository.Enumerate(includeDeleted);
    }
}
