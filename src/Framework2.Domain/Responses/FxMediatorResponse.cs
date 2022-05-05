using Framework2.Domain.Core.Responses;

namespace Framework2.Domain.Core.Responses
{
    public abstract class FxMediatorResponse : FxResponse { }

    public abstract class FxQueryResponse : FxMediatorResponse { }
    public abstract class FxCommandResponse : FxMediatorResponse { }
}
