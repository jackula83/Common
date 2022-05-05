using Framework2.Domain.Core.Handlers;
using Framework2.Domain.Core.Identity;
using Framework2.Domain.UnitTests.Models.Stubs;
using Framework2.Infra.Data.UnitTests.Tests.Data.Stubs;
using Framework2.Infra.Data.UnitTests.Tests.Models.Stubs;
using Microsoft.Extensions.Logging;

namespace Framework2.Domain.UnitTests.Handlers.Stubs
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
