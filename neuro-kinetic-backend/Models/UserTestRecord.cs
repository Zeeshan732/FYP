using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class UserTestRecord
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; } = string.Empty;
        
        public DateTime TestDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        [MaxLength(50)]
        public string TestResult { get; set; } = "Uncertain"; // "Positive", "Negative", "Uncertain"
        
        public double Accuracy { get; set; } = 0.0; // Percentage (0-100)
        
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // "Completed", "Pending", "Failed"
        
        [MaxLength(500)]
        public string? VoiceRecordingUrl { get; set; }
        
        public string? AnalysisNotes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property (optional)
        public User? User { get; set; }
    }
}



