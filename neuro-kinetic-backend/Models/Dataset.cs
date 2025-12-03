using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class Dataset
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? Source { get; set; }
        
        [MaxLength(50)]
        public string? Version { get; set; }
        
        public int? TotalSamples { get; set; }
        
        public int? VoiceSamples { get; set; }
        
        public int? GaitSamples { get; set; }
        
        public int? MultiModalSamples { get; set; }
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [MaxLength(200)]
        public string? License { get; set; }
        
        [MaxLength(500)]
        public string? AccessLink { get; set; }
        
        public bool IsPublic { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [MaxLength(200)]
        public string? Citation { get; set; }
    }
}



