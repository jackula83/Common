namespace Common.Application.Core.Models
{
    public abstract class FxEvent : FxRequest
    {
        public string Name => GetType().Name;
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; }

        public FxEvent()
        {
            var @type = GetType();

            Source = @type.Assembly.GetName().Name;
            Timestamp = DateTime.UtcNow;
        }
    }
}
