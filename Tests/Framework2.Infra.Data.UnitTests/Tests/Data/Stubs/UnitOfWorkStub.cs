using Framework2.Infra.Data.Repository;

namespace Framework2.Infra.Data.UnitTests.Tests.Data.Stubs
{
    public class UnitOfWorkStub : FxUnitOfWork<SqlServerDbContextStub>
    {
        public EntityRepositoryStub Repository { get; set; }

        public UnitOfWorkStub(SqlServerDbContextStub context) : base(context)
            => Repository = new(context);
    }
}
