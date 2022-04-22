using Common.Application.Core.Models;
using Common.Domain.UnitTests.Models.Stubs;

namespace Common.Application.UnitTests.Tests.Models.Stubs
{
    public class EntityCommandRequestStub : FxEntityCommandRequest<EntityStub>
    {
        public override void From<TControllerRequest>(TControllerRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
