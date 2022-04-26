using Common.Domain.Core.Handlers;
using Common.Domain.UnitTests.Models.Stubs;
using Common.Domain.UnitTests.Tests.Data.Stubs;
using Microsoft.Extensions.Logging;

namespace Common.Domain.UnitTests.Handlers.Stubs
{
    internal class EntityQueryHandlerStub : FxEntityQueryHandler<EntityQueryRequestStub, EntityQueryResponseStub, EntityStub>
    {
        public EntityQueryHandlerStub(ILogger<EntityQueryHandlerStub> logger, IEntityRepositoryStub repository)
            : base(logger, repository)
        {
        }
    }
}
