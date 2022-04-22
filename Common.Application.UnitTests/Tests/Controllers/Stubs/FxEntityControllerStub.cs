using Common.Application.Core.Controllers;
using Common.Application.UnitTests.Tests.Models.Stubs;
using Common.Domain.UnitTests.Tests.Models.Stubs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Application.UnitTests.Tests.Controllers.Stubs
{
    public class FxEntityControllerStub : FxEntityController<EntityQueryRequestStub, EntityCommandRequestStub, EntityStub>
    {
        public FxEntityControllerStub(IMediator mediator, ILogger<FxEntityControllerStub> logger) : base(mediator, logger)
        {
        }
    }
}
