using Common.Application.Core.Models;

namespace Common.Application.UnitTests.Tests.Models.Stubs
{
    public class EntityQueryRequestStub : FxEntityQueryRequest
    {
        public override void From<TControllerRequest>(TControllerRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
