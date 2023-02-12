using System.Text.Json.Serialization;

namespace Framework2.Infra.MQ.Dapr
{
    internal record DaprData<T> ([property: JsonPropertyName("data")] T Data);
}
