using Common.Application.Core.Interfaces;

namespace Common.Application.Core.Utils
{
    public sealed class FxEnvironment : IEnvironment
    {
        public string? Get(string key)
            => System.Environment.GetEnvironmentVariable(key);

        public void Set(string key, string value)
            => System.Environment.SetEnvironmentVariable(key, value);
    }
}
