using Framework2.Core.Extensions;
using Framework2.Domain.Core.Requests;
using Framework2.Infra.Data.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Framework2.Application.Core.Controllers
{
    public abstract class FxEntityController<TQueryRequest, TCommandRequest, TDataObject> : FxController
        where TQueryRequest : FxEntityQueryRequest, new()
        where TCommandRequest : FxEntityCommandRequest<TDataObject>, new()
        where TDataObject : IDataObject
    {
        protected FxEntityController(IMediator mediator, ILogger<FxEntityController<TQueryRequest, TCommandRequest, TDataObject>> logger)
            : base(mediator, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => await this.Send(new TQueryRequest());

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
            => await this.Send(new TQueryRequest { Id = id });

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TCommandRequest request)
            => await this.Send(request);

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromBody] TCommandRequest request)
        {
            if (request.Item == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(request)), nameof(FxEntityController<TQueryRequest, TCommandRequest, TDataObject>));
                return BadRequest();
            }

            return await this.Send(request.Tap(x => x.Item!.DeleteFlag = true));
        }
    }
}
