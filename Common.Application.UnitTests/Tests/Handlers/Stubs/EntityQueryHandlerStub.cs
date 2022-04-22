using Common.Application.Core.Handlers;
using Common.Application.UnitTests.Tests.Models.Stubs;
using Common.Domain.UnitTests.Interfaces;
using Common.Domain.UnitTests.Tests.Models.Stubs;

namespace Common.Application.UnitTests.Tests.Handlers.Stubs
{
    internal class EntityQueryHandlerStub : FxEntityQueryHandler<EntityQueryRequestStub, EntityQueryResponseStub, EntityStub>
    {
        public EntityQueryHandlerStub(IEntityRepositoryStub repository) : base(repository)
        {
        }
    }
}
