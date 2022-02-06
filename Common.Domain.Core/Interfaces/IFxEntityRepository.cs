using Common.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Core.Interfaces
{
    public interface IFxEntityRepository
    {
    }

    public interface IFxEntityRepository<TEntityType> : IFxEntityRepository
        where TEntityType : FxEntity
    {
        Task<int> Add(TEntityType entity);
        Task<List<TEntityType>> Enumerate(bool includeDeleted);
        Task<TEntityType?> Get(int id);
        Task<bool> Update(TEntityType entity);
        Task<bool> Delete(int id);
        Task<bool> Delete(TEntityType entity);
    }
}
