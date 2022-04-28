namespace Framework2.Domain.Core.Requests
{
    public abstract class FxMediatorRequest : FxRequest
    {
    }

    public abstract class FxQueryRequest : FxMediatorRequest { }
    public abstract class FxCommandRequest : FxMediatorRequest { }
}
