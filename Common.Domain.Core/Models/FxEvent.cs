namespace Common.Domain.Core.Models
{
    public abstract class FxEvent : FxRequest
    {
        public virtual string Name { get; set; }
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; }

        public FxEvent()
        {
            var @type = this.GetType();

            this.Name = @type.Name;
            this.Source = @type.Assembly.GetName().Name;
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
