using Framework2.Domain.Core.Data;

namespace Framework2.Domain.UnitTests.Tests.Data.Stubs
{
    public class UnitOfWorkStub : FxUnitOfWork<SqlServerDbContextStub>
    {
        public EntityRepositoryStub Repository { get; set; }

        public UnitOfWorkStub(SqlServerDbContextStub context) : base(context)
            => Repository = new(context);
    }
}
