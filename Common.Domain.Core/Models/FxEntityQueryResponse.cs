﻿namespace Common.Domain.Core.Models
{
    public abstract class FxEntityQueryResponse : FxQueryResponse { }

    public abstract class FxEntityQueryResponse<TEntity> : FxEntityQueryResponse
        where TEntity : FxEntity
    {
        public List<TEntity> Items { get; set; } = new();

        public TEntity? Item => Items?.FirstOrDefault();
    }
}
