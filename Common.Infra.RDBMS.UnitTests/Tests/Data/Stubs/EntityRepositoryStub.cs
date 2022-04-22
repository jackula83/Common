using Common.Domain.UnitTests.Interfaces;
using Common.Domain.UnitTests.Models.Stubs;
using Common.Infra.RDBMS.Data;

namespace Common.Infra.RDBMS.UnitTests.Tests.Data.Stubs
{
    public class EntityRepositoryStub : FxEntityRepository<SqlServerDbContextStub, EntityStub>, IEntityRepositoryStub
    {
        public EntityRepositoryStub(SqlServerDbContextStub context) : base(context)
        {
        }
    }
}
