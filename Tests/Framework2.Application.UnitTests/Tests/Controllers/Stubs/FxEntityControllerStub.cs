using Framework2.Application.Core.Controllers;
using Framework2.Domain.UnitTests.Models.Stubs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Framework2.Application.UnitTests.Tests.Controllers.Stubs
{
    public class FxEntityControllerStub : FxEntityController<EntityQueryRequestStub, EntityCommandRequestStub, EntityStub>
    {
        public FxEntityControllerStub(IMediator mediator, ILogger<FxEntityControllerStub> logger) : base(mediator, logger)
        {
        }
    }
}
