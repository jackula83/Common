using Framework2.Domain.Core.Data;
using Framework2.Domain.UnitTests.Models.Stubs;

namespace Framework2.Domain.UnitTests.Tests.Data.Stubs
{
    public class EntityRepositoryStub : FxEntityRepository<SqlServerDbContextStub, EntityStub>, IEntityRepositoryStub
    {
        public EntityRepositoryStub(SqlServerDbContextStub context) : base(context)
        {
        }
    }
}
