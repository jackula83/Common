using Framework2.Core.Extensions;
using Framework2.Domain.Core.Identity;
using Framework2.Domain.Core.Requests;
using Framework2.Domain.Core.Responses;
using Framework2.Infra.Data.Entity;
using Framework2.Infra.Data.Repository;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.Core.Handlers;

public abstract class FxEntityCommandHandler<TRequest, TResponse, TAggregateRoot> : FxCommandHandler<TRequest, TResponse>
   where TRequest : FxEntityCommandRequest<TAggregateRoot>
   where TResponse : FxEntityCommandResponse, new()
   where TAggregateRoot : class, IAggregateRoot
{
    protected readonly IEntityRepository<TAggregateRoot> _repository;
    protected readonly IUserIdentity _identity;

    protected abstract bool HasPermission();

    public FxEntityCommandHandler(
        IUserIdentity identity,
        ILogger<FxEntityCommandHandler<TRequest, TResponse, TAggregateRoot>> logger,
        IEntityRepository<TAggregateRoot> repository)
        : base(logger)
    {
        _repository = repository;
        _identity = identity;
    }

    protected override async Task<TResponse> ExecuteAsync(TRequest command, CancellationToken cancellationToken = default)
    {
#pragma warning disable IDE0059
        var result = default(TResponse);
#pragma warning restore IDE0059

        if (command.Item?.Id == 0)
            result = new TResponse { Success = true, Item = await Add(command.Item, true) };
        else
        {
            var updatedEntity = await Update(command.Item!, true);
            result = new TResponse { Success = updatedEntity != default, Item = updatedEntity };
        }

        return result;
    }

    protected virtual async Task<TAggregateRoot> Add(TAggregateRoot entity, bool commitImmediately = false)
    {
        if (!HasPermission())
            throw new UnauthorizedAccessException();

        var result = await _repository.Add(
            entity
                .Tap(x => x.CreatedBy = _identity.UserName)
                .Tap(x => x.UpdatedBy = _identity.UserName));

        if (commitImmediately)
            await _repository.Save();

        return result;

    }

    protected virtual async Task<TAggregateRoot?> Update(TAggregateRoot model, bool commitImmediately = false)
    {
        if (!HasPermission())
            throw new UnauthorizedAccessException();

        var result = await _repository.Update(
            model.Tap(x => x.UpdatedBy = _identity.UserName));

        if (commitImmediately)
            await _repository.Save();

        return result;
    }
}
