using Common.Domain.Core.Handlers;
using Common.Domain.Core.Interfaces;
using Common.Domain.Tests.Unit.Models.Stubs;
using Common.Domain.UnitTests.Interfaces;
using Common.Domain.UnitTests.Unit.Models.Stubs;

namespace Common.Domain.UnitTests.Unit.Handlers.Stubs
{
    internal class EntityCommandHandlerStub : FxEntityCommandHandler<EntityCommandRequestStub, EntityCommandResponseStub, EntityStub>
    {
        public EntityCommandHandlerStub(IUserIdentity identity, IEntityRepositoryStub repository) : base(identity, repository)
        {
        }

        public bool WritePermission { get; set; } = false;

        protected override bool HasPermission()
            => WritePermission;
    }
}
