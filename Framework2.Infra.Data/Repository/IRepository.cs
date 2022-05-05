using Framework2.Infra.Data.Entity;

namespace Framework2.Infra.Data.Repository
{
    public interface IRepository
    {
    }

    public interface IEntityRepository<TDataObject> : IRepository
        where TDataObject : IDataObject
    {
        Task<TDataObject> Add(TDataObject entity);
        Task<List<TDataObject>> Enumerate(bool includeDeleted = false);
        Task<TDataObject?> Get(int id);
        Task<TDataObject?> Update(TDataObject entity);
        Task<TDataObject?> Delete(int id);
        Task<TDataObject?> Delete(TDataObject entity);
        Task Save();
    }
}
