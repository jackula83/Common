namespace Framework2.Infra.MQ.Core
{
    public abstract class FxEnv
    {
        protected static string BuildEnvName(string prefix, string name)
            => $"{prefix}_{name}".ToUpper();
    }
}
