using Common.Domain.Core.Extensions;
using Common.Domain.Core.Interfaces;
using Common.Domain.Core.Models;

namespace Common.Domain.Core.Handlers
{
    public abstract class FxEntityCommandHandler<TRequest, TResponse, TEntity> : FxCommandHandler<TRequest, TResponse>
       where TRequest : FxEntityCommandRequest<TEntity>
       where TResponse : FxEntityCommandResponse, new()
       where TEntity : FxEntity
    {
        protected readonly IEntityRepository<TEntity> _repository;
        protected readonly IUserIdentity _identity;

        protected abstract bool HasPermission();

        public FxEntityCommandHandler(IUserIdentity identity, IEntityRepository<TEntity> repository)
        {
            _repository = repository;
            _identity = identity;
        }

        protected override async Task<TResponse> ExecuteAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            var result = default(TResponse);

            if (command.Item?.Id == 0)
                result = new TResponse { Success = true, Item = await this.Add(command.Item, true) };
            else
            {
                var updatedEntity = await this.Update(command.Item!, true);
                result = new TResponse { Success = updatedEntity != default, Item = updatedEntity };
            }

            return result;
        }

        protected virtual async Task<TEntity> Add(TEntity entity, bool commitImmediately = false)
        {
            if (!this.HasPermission())
                throw new UnauthorizedAccessException();

            var result = await _repository.Add(
                entity
                    .Tap(x => x.CreatedBy = _identity.UserName)
                    .Tap(x => x.UpdatedBy = _identity.UserName));

            if (commitImmediately)
                await _repository.Save();

            return result;

        }

        protected virtual async Task<TEntity?> Update(TEntity model, bool commitImmediately = false)
        {
            if (!this.HasPermission())
                throw new UnauthorizedAccessException();

            var result = await _repository.Update(
                model.Tap(x => x.UpdatedBy = _identity.UserName));

            if (commitImmediately)
                await _repository.Save();

            return result;
        }
    }
}
