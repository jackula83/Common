using Framework2.Infra.MQ.Core;

namespace Framework2.Infra.MQ.Dapr
{
    public sealed class DaprConfig : FxEnv
    {
        private static string Prefix => "Dapr_Pubsub";

        public static string BaseUrl => BuildEnvName(Prefix, nameof(BaseUrl));

        public static string Port => BuildEnvName(Prefix, nameof(Port));

        public string PubSubName { get; private set; }

        public DaprConfig(string pubSubName)
        {
            this.PubSubName = pubSubName;
        }
    }
}
