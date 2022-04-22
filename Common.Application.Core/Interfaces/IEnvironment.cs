namespace Common.Application.Core.Interfaces
{
    public interface IEnvironment
    {
        string? Get(string key);
        void Set(string key, string value);
    }
}
