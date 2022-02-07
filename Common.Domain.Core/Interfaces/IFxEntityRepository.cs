using Common.Domain.Core.Models;
namespace Common.Domain.Core.Interfaces
{
    public interface IFxEntityRepository
    {
    }

    public interface IFxEntityRepository<TEntity> : IFxEntityRepository
        where TEntity : FxEntity
    {
        Task<int> Add(TEntity entity);
        Task<List<TEntity>> Enumerate(bool includeDeleted);
        Task<TEntity?> Get(int id);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(TEntity entity);
    }
}
