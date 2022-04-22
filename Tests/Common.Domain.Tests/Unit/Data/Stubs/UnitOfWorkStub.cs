using Common.Domain.Core.Data;

namespace Common.Domain.Tests.Unit.Data.Stubs
{
    public class UnitOfWorkStub : FxUnitOfWork<SqlServerDbContextStub>
    {
        public EntityRepositoryStub Repository { get; set; }

        public UnitOfWorkStub(SqlServerDbContextStub context) : base(context)
            => this.Repository = new(context);
    }
}
