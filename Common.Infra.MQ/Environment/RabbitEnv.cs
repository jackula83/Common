using Common.Infra.MQ.Environment.Abstracts;

namespace Common.Infra.MQ.Environment
{
    public sealed class RabbitEnv : EnvVars
    {
        public const string Hostname = nameof(Hostname);
    }
}
