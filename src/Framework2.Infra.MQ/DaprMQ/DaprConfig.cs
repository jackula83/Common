
using Framework2.Infra.MQ.Core;

namespace Framework2.Infra.MQ.DaprMQ
{
    public sealed class DaprConfig : FxEnv
    {
        private static string Prefix => "Dapr_Mq";

        public static string QueueName => BuildEnvName(Prefix, nameof(QueueName));
    }
}
