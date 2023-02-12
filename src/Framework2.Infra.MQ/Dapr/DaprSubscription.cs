using System.Text.Json.Serialization;

namespace Framework2.Infra.MQ.Dapr
{
    internal record DaprSubscription(
        [property: JsonPropertyName("pubsubname")] string PubSubName,
        [property: JsonPropertyName("topic")] string Topic,
        [property: JsonPropertyName("route")] string Route
    );
}
