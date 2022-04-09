namespace Common.Infra.MQ.Environment.Abstracts
{
    public abstract class EnvVars
    {
        protected static string BuildEnvName(string prefix, string name)
            => $"{prefix}_{name}".ToUpper();
    }
}
