using Framework2.Domain.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

namespace Framework2.Application.Core.Controllers
{
    public abstract class FxController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger<FxController> _logger;

        protected FxController(IMediator mediator, ILogger<FxController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("health/")]
        public IActionResult Health()
            => Ok(this.GetType().Name + " is working!");

        protected ContentResult JsonContent(object content)
            => Content(JsonConvert.SerializeObject(content), MediaTypeNames.Application.Json);

        protected async Task<IActionResult> Send<TRequest>(TRequest request)
            where TRequest : FxMediatorRequest, new()
        {
            try
            {
                var response = await _mediator.Send(request);
                return this.JsonContent(response!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(FxController));
            }

            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}
