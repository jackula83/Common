namespace Common.Application.Core.Models
{
    public abstract class FxMediatorResponse : FxResponse { }

    public abstract class FxQueryResponse : FxMediatorResponse { }
    public abstract class FxCommandResponse : FxMediatorResponse { }
}
