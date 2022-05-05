namespace Framework2.Infra.Data.Entity
{
    public interface IDataObject
    {
        int Id { get; set; }

        public Guid Uuid { get; set; }

        bool DeleteFlag { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
