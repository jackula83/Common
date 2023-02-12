using Framework2.Infra.MQ.Core;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Framework2.Infra.MQ.Dapr
{
    public class DaprQueue
    {
        private readonly HttpClient _httpClient;
        private readonly DaprConfig _config;

        public DaprQueue(DaprConfig config)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            _config = config;
        }

        public async Task Publish<TEvent>(TEvent @event)
            where TEvent : FxEvent
        {
            var payloadJson = JsonConvert.SerializeObject(@event);
            var content = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            _httpClient.PostAsync(
                $"{DaprConfig.BaseUrl}/v1.0/publish/{_config.PubSubName}/{@event.Name}",
                content);

            await Task.CompletedTask;
        }
    }
}
