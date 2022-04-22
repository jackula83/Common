using Common.Domain.Core.Models;
using Common.Domain.Tests.Unit.Models.Stubs;

namespace Common.Domain.UnitTests.Unit.Models.Stubs
{
    public class EntityCommandRequestStub : FxEntityCommandRequest<EntityStub>
    {
        public override void From<TControllerRequest>(TControllerRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
