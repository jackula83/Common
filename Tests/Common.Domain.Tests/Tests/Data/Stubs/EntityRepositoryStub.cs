using Common.Domain.Core.Data;
using Common.Domain.UnitTests.Models.Stubs;

namespace Common.Domain.UnitTests.Tests.Data.Stubs
{
    public class EntityRepositoryStub : FxEntityRepository<SqlServerDbContextStub, EntityStub>, IEntityRepositoryStub
    {
        public EntityRepositoryStub(SqlServerDbContextStub context) : base(context)
        {
        }
    }
}
