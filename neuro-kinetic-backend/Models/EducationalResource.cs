using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class EducationalResource
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string? Description { get; set; }
        
        public ResourceType Type { get; set; }
        
        [MaxLength(500)]
        public string? ContentJson { get; set; }
        
        [MaxLength(500)]
        public string? FilePath { get; set; }
        
        [MaxLength(500)]
        public string? ImageUrl { get; set; }
        
        public int? DisplayOrder { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        [MaxLength(200)]
        public string? Tags { get; set; }
    }
    
    public enum ResourceType
    {
        Timeline = 0,
        Explainer = 1,
        CaseStudy = 2,
        Tutorial = 3,
        Video = 4
    }
}

