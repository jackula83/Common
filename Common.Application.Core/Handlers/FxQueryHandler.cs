﻿using Common.Application.Core.Models;
using Microsoft.Extensions.Logging;

namespace Common.Application.Core.Handlers
{
    public abstract class FxQueryHandler<TRequest, TResponse> : FxHandler<TRequest, TResponse>
        where TRequest : FxQueryRequest
        where TResponse : FxQueryResponse, new()
    {
        protected FxQueryHandler(ILogger<FxQueryHandler<TRequest, TResponse>> logger) : base(logger)
        {
        }
    }
}
