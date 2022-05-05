using Framework2.Infra.Data.Entity;

namespace Framework2.Infra.Data.Repository
{
    public interface IRepository
    {
    }

    public interface IEntityRepository<TAggregateRoot> : IRepository
        where TAggregateRoot : IAggregateRoot
    {
        Task<TAggregateRoot> Add(TAggregateRoot entity);
        Task<List<TAggregateRoot>> Enumerate(bool includeDeleted = false);
        Task<TAggregateRoot?> Get(int id);
        Task<TAggregateRoot?> Update(TAggregateRoot entity);
        Task<TAggregateRoot?> Delete(int id);
        Task<TAggregateRoot?> Delete(TAggregateRoot entity);
        Task Save();
    }
}
