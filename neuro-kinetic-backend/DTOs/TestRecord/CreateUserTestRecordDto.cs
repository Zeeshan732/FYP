using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.DTOs.TestRecord
{
    public class CreateUserTestRecordDto
    {
        public int? UserId { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string? TestResult { get; set; } // "Positive", "Negative", "Uncertain"
        
        [Range(0, 100)]
        public double? Accuracy { get; set; } // 0-100
        
        [MaxLength(50)]
        public string? Status { get; set; } // "Completed", "Pending", "Failed"
        
        [MaxLength(500)]
        public string? VoiceRecordingUrl { get; set; }
        
        [MaxLength(5000)]
        public string? AnalysisNotes { get; set; }
    }
}

