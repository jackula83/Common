namespace Framework2.Application.Core.Env
{
    public interface IEnvironment
    {
        string? Get(string key);
        void Set(string key, string value);
    }
}
