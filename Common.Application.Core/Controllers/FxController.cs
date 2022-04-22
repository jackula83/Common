using Common.Application.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Common.Application.Core.Controllers
{
    public abstract class FxController<TControllerRequest, TQueryRequest, TCommandRequest> : ControllerBase
           where TControllerRequest : FxControllerRequest
           where TQueryRequest : FxQueryRequest, new()
           where TCommandRequest : FxCommandRequest, new()
    {
        protected readonly IMediator _mediator;

        protected FxController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
