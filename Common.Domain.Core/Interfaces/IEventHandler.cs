namespace Common.Domain.Core.Interfaces
{
    public interface IEventHandler
    {
        Type EventHandled { get; }
    }
}
