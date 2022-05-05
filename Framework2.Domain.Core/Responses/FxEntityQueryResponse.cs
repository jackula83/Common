using Framework2.Infra.Data.Entity;

namespace Framework2.Domain.Core.Responses
{
    public abstract class FxEntityQueryResponse : FxQueryResponse { }

    public abstract class FxEntityQueryResponse<TAggregateRoot> : FxEntityQueryResponse
        where TAggregateRoot : class, IAggregateRoot
    {
        public List<TAggregateRoot> Items { get; set; } = new();

        public TAggregateRoot? Item => Items?.FirstOrDefault();
    }
}
