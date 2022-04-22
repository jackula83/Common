using Common.Application.Core.Handlers;
using Common.Application.Core.Interfaces;
using Common.Application.UnitTests.Tests.Models.Stubs;
using Common.Domain.UnitTests.Interfaces;
using Common.Domain.UnitTests.Models.Stubs;
using Microsoft.Extensions.Logging;

namespace Common.Application.UnitTests.Tests.Handlers.Stubs
{
    internal class EntityCommandHandlerStub : FxEntityCommandHandler<EntityCommandRequestStub, EntityCommandResponseStub, EntityStub>
    {
        public EntityCommandHandlerStub(IUserIdentity identity, ILogger<EntityCommandHandlerStub> logger, IEntityRepositoryStub repository) 
            : base(identity, logger, repository)
        {
        }

        public bool WritePermission { get; set; } = false;

        protected override bool HasPermission()
            => WritePermission;
    }
}
