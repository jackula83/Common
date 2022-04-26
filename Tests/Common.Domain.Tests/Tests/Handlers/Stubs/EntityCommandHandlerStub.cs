using Common.Domain.Core.Handlers;
using Common.Domain.Core.Identity;
using Common.Domain.UnitTests.Models.Stubs;
using Common.Domain.UnitTests.Tests.Data.Stubs;
using Microsoft.Extensions.Logging;

namespace Common.Domain.UnitTests.Handlers.Stubs
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
