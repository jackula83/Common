using Common.Domain.Core.Handlers;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.UnitTests.Interfaces;
using Common.Domain.UnitTests.Unit.Models.Stubs;

namespace Common.Domain.UnitTests.Unit.Handlers.Stubs
{
    internal class EntityQueryHandlerStub : FxEntityQueryHandler<EntityQueryRequestStub, EntityQueryResponseStub, EntityStub>
    {
        public EntityQueryHandlerStub(IEntityRepositoryStub repository) : base(repository)
        {
        }
    }
}
