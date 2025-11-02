using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class UploadedFile
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(1000)]
        public string FilePath { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; } = string.Empty;
        
        public long FileSize { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FileType { get; set; } = string.Empty; // "voice", "gait", "image", "video"
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [MaxLength(100)]
        public string? SessionId { get; set; }
        
        public int? UploadedByUserId { get; set; }
        
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
    }
}

