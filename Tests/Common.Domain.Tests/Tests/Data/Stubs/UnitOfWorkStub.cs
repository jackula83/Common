using Common.Domain.Core.Data;

namespace Common.Domain.UnitTests.Tests.Data.Stubs
{
    public class UnitOfWorkStub : FxUnitOfWork<SqlServerDbContextStub>
    {
        public EntityRepositoryStub Repository { get; set; }

        public UnitOfWorkStub(SqlServerDbContextStub context) : base(context)
            => Repository = new(context);
    }
}
