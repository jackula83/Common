using Framework2.Infra.Data.Repository;
using Framework2.Infra.Data.UnitTests.Tests.Models.Stubs;

namespace Framework2.Infra.Data.UnitTests.Tests.Data.Stubs
{
    public class EntityRepositoryStub : FxRepository<SqlServerDbContextStub, EntityStub>, IEntityRepositoryStub
    {
        public EntityRepositoryStub(SqlServerDbContextStub context) : base(context)
        {
        }
    }
}
