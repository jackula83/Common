using System.ComponentModel.DataAnnotations;

namespace Common.Domain.Core.Models
{
    public abstract class FxEntity : FxModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid Uuid { get; set; }

        [Required]
        public bool DeleteFlag { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(60)]
        public string? CreatedBy { get; set; }

        [StringLength(60)]
        public string? UpdatedBy { get; set; }
    }
}
