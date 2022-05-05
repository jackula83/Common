namespace Framework2.Infra.MQ.Core
{
    public abstract class FxEvent
    {
        public string CorrelationId { get; set; }
        public string Name => GetType().Name;
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; }

        public FxEvent()
        {
            var @type = GetType();

            Source = @type.Assembly.GetName().Name;
            Timestamp = DateTime.UtcNow;
            CorrelationId = Guid.NewGuid().ToString();
        }
    }
}
