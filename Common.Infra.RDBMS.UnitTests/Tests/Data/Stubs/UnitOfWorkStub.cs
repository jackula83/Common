using Common.Infra.RDBMS.Data;

namespace Common.Infra.RDBMS.UnitTests.Tests.Data.Stubs
{
    public class UnitOfWorkStub : FxUnitOfWork<SqlServerDbContextStub>
    {
        public EntityRepositoryStub Repository { get; set; }

        public UnitOfWorkStub(SqlServerDbContextStub context) : base(context)
            => Repository = new(context);
    }
}
