using Common.Domain.Core.Data;
using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Unit.Models.Stubs;

namespace Common.Domain.Tests.Unit.Data.Stubs
{
    internal class EntityRepositoryStub : FxEntityRepository<SqlServerDbContextStub, EntityStub>, IFxEntityRepository<EntityStub>
    {
        public EntityRepositoryStub(SqlServerDbContextStub context) : base(context)
        {
        }
    }
}
