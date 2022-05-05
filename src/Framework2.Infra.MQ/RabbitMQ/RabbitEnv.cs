using Framework2.Infra.MQ.Core;

namespace Framework2.Infra.MQ.RabbitMQ
{
    public sealed class RabbitEnv : FxEnv
    {
        private static string Prefix => "Rabbit_Mq";

        public static string Hostname => BuildEnvName(Prefix, nameof(Hostname));
        public static string Username => BuildEnvName(Prefix, nameof(Username));
        public static string Password => BuildEnvName(Prefix, nameof(Password));
    }
}
