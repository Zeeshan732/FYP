using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class TechnicalDocument
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(5000)]
        public string? Content { get; set; }
        
        [MaxLength(100)]
        public string? Category { get; set; }
        
        public DocumentType Type { get; set; }
        
        [MaxLength(500)]
        public string? FilePath { get; set; }
        
        [MaxLength(500)]
        public string? ApiEndpoint { get; set; }
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public UserRole? MinimumAccessLevel { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public bool IsPublished { get; set; } = false;
    }
    
    public enum DocumentType
    {
        API = 0,
        Architecture = 1,
        Deployment = 2,
        Integration = 3,
        General = 4
    }
}



