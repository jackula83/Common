using Framework2.Infra.MQ.Core;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Framework2.Infra.MQ.Dapr
{
    public class DaprQueue : EventQueue, IEventQueue
    {
        private readonly HttpClient _httpClient;
        private readonly DaprConfig _config;

        public DaprQueue(IServiceProvider provider, HttpClient httpClient, DaprConfig config)
            : base(provider)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            _config = config;
        }

        public override Task<uint> Count<TEvent>()
        {
            throw new NotSupportedException("Dapr does not support the count operation");
        }

        public override async Task Publish<TEvent>(TEvent @event)
        {
            var payloadJson = JsonConvert.SerializeObject(@event);
            var content = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            _httpClient.PostAsync(
                $"{DaprConfig.BaseUrl}/v1.0/publish/{_config.PubSubName}/{@event.Name}",
                content);

            await Task.CompletedTask;
        }

        protected override Task StartConsumingEvents<TEvent>(string eventName)
        {
            throw new NotImplementedException();
        }
    }
}
