using Common.Domain.Core.Models;
namespace Common.Domain.Core.Interfaces
{
    public interface IFxEntityRepository
    {
    }

    public interface IFxEntityRepository<TEntity> : IFxEntityRepository
        where TEntity : FxEntity
    {
        Task<TEntity> Add(TEntity entity);
        Task<List<TEntity>> Enumerate(bool includeDeleted = false);
        Task<TEntity?> Get(int id);
        Task<TEntity?> Update(TEntity entity);
        Task<TEntity?> Delete(int id);
        Task<TEntity?> Delete(TEntity entity);
    }
}
