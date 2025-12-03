using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.DTOs.Analysis
{
    public class AnalysisRequest
    {
        [Required]
        public string SessionId { get; set; } = string.Empty;
        
        public bool HasVoiceData { get; set; } = false;
        
        public bool HasGaitData { get; set; } = false;
        
        public string? VoiceDataJson { get; set; }
        
        public string? GaitDataJson { get; set; }
        
        public string? WaveformDataJson { get; set; }
        
        public string? SkeletonDataJson { get; set; }
    }
}



