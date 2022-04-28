using Framework2.Domain.Core.Handlers;
using Framework2.Domain.UnitTests.Models.Stubs;
using Framework2.Domain.UnitTests.Tests.Data.Stubs;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.UnitTests.Handlers.Stubs
{
    internal class EntityQueryHandlerStub : FxEntityQueryHandler<EntityQueryRequestStub, EntityQueryResponseStub, EntityStub>
    {
        public EntityQueryHandlerStub(ILogger<EntityQueryHandlerStub> logger, IEntityRepositoryStub repository)
            : base(logger, repository)
        {
        }
    }
}
