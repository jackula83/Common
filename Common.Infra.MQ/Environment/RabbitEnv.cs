using Common.Infra.MQ.Environment.Abstracts;

namespace Common.Infra.MQ.Environment
{
    public sealed class RabbitEnv : EnvVars
    {
        private static string Prefix => "Rabbit_Mq";

        public static string Hostname => BuildEnvName(Prefix, nameof(Hostname));
        public static string Username => BuildEnvName(Prefix, nameof(Username));
        public static string Password => BuildEnvName(Prefix, nameof(Password));
    }
}
