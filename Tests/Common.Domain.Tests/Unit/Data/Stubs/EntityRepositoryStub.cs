using Common.Domain.Core.Data;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.UnitTests.Interfaces;

namespace Common.Domain.Tests.Unit.Data.Stubs
{
    internal class EntityRepositoryStub : FxEntityRepository<SqlServerDbContextStub, EntityStub>, IEntityRepositoryStub
    {
        public EntityRepositoryStub(SqlServerDbContextStub context) : base(context)
        {
        }
    }
}
