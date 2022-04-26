using Common.Domain.Core.Data;
using Common.Domain.Core.Extensions;
using Common.Domain.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Common.Application.Core.Controllers
{
    public abstract class FxEntityController<TQueryRequest, TCommandRequest, TEntity> : FxController
        where TQueryRequest : FxEntityQueryRequest, new()
        where TCommandRequest : FxEntityCommandRequest<TEntity>, new()
        where TEntity : FxEntity
    {
        protected FxEntityController(IMediator mediator, ILogger<FxEntityController<TQueryRequest, TCommandRequest, TEntity>> logger)
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
                _logger.LogError(new ArgumentNullException(nameof(request)), nameof(FxEntityController<TQueryRequest, TCommandRequest, TEntity>));
                return BadRequest();
            }

            return await this.Send(request.Tap(x => x.Item!.DeleteFlag = true));
        }
    }
}
