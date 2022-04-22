using Common.Application.Core.Handlers;
using Common.Application.UnitTests.Tests.Models.Stubs;
using Common.Domain.UnitTests.Interfaces;
using Common.Domain.UnitTests.Models.Stubs;
using Microsoft.Extensions.Logging;

namespace Common.Application.UnitTests.Tests.Handlers.Stubs
{
    internal class EntityQueryHandlerStub : FxEntityQueryHandler<EntityQueryRequestStub, EntityQueryResponseStub, EntityStub>
    {
        public EntityQueryHandlerStub(ILogger<EntityQueryHandlerStub> logger, IEntityRepositoryStub repository) 
            : base(logger, repository)
        {
        }
    }
}
