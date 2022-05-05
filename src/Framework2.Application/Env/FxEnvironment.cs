namespace Framework2.Application.Core.Env
{
    public sealed class FxEnvironment : IEnvironment
    {
        public string? Get(string key)
            => Environment.GetEnvironmentVariable(key);

        public void Set(string key, string value)
            => Environment.SetEnvironmentVariable(key, value);
    }
}
