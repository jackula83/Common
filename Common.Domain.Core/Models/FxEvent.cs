namespace Common.Domain.Core.Models
{
    public abstract class FxEvent : FxRequest
    {
        public string Name => this.GetType().Name;
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; }

        public FxEvent()
        {
            var @type = this.GetType();

            this.Source = @type.Assembly.GetName().Name;
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
