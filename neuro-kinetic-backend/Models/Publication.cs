using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace neuro_kinetic_backend.Models
{
    public class Publication
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(2000)]
        public string? Abstract { get; set; }
        
        [MaxLength(1000)]
        public string? Authors { get; set; }
        
        [MaxLength(200)]
        public string? Journal { get; set; }
        
        [MaxLength(50)]
        public string? Year { get; set; }
        
        [MaxLength(500)]
        public string? DOI { get; set; }
        
        [MaxLength(500)]
        public string? Link { get; set; }
        
        public PublicationType Type { get; set; }
        
        public bool IsFeatured { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        [MaxLength(100)]
        public string? Tags { get; set; }
    }
    
    public enum PublicationType
    {
        Journal = 0,
        Conference = 1,
        Preprint = 2,
        TechnicalReport = 3
    }
}



