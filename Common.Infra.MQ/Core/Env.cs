namespace Common.Infra.MQ.Core
{
    public abstract class Env
    {
        protected static string BuildEnvName(string prefix, string name)
            => $"{prefix}_{name}".ToUpper();
    }
}
