using Common.Domain.Core.Models;

namespace Common.Domain.UnitTests.Unit.Models.Stubs
{
    public class EntityQueryRequestStub : FxEntityQueryRequest
    {
        public override void From<TControllerRequest>(TControllerRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
