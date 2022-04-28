namespace Framework2.Domain.Core.Data
{
    public interface IEntityRepository
    {
    }

    public interface IEntityRepository<TEntity> : IEntityRepository
        where TEntity : FxEntity
    {
        Task<TEntity> Add(TEntity entity);
        Task<List<TEntity>> Enumerate(bool includeDeleted = false);
        Task<TEntity?> Get(int id);
        Task<TEntity?> Update(TEntity entity);
        Task<TEntity?> Delete(int id);
        Task<TEntity?> Delete(TEntity entity);
        Task Save();
    }
}
