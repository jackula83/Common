using Framework2.Domain.Core.Requests;
using Framework2.Domain.Core.Responses;
using Framework2.Infra.Data.Entity;
using Framework2.Infra.Data.Repository;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Core.Handlers
{
    public abstract class FxEntityQueryHandler<TRequest, TResponse, TDataObject> : FxQueryHandler<TRequest, TResponse>
        where TRequest : FxEntityQueryRequest
        where TResponse : FxEntityQueryResponse<TDataObject>, new()
        where TDataObject : class, IDataObject
    {
        protected readonly IEntityRepository<TDataObject> _repository;

        public FxEntityQueryHandler(
            ILogger<FxEntityQueryHandler<TRequest, TResponse, TDataObject>> logger,
            IEntityRepository<TDataObject> repository)
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

        protected virtual async Task<TDataObject?> GetSingle(int id)
            => await _repository.Get(id);

        protected virtual async Task<List<TDataObject>> GetCollection(bool includeDeleted = false)
            => await _repository.Enumerate(includeDeleted);
    }
}
