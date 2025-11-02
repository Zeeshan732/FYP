using System.ComponentModel.DataAnnotations;

namespace neuro_kinetic_backend.Models
{
    public class AnalysisResult
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string SessionId { get; set; } = string.Empty;
        
        public AnalysisType AnalysisType { get; set; }
        
        public bool HasVoiceData { get; set; } = false;
        
        public bool HasGaitData { get; set; } = false;
        
        public decimal? PredictionScore { get; set; }
        
        public decimal? ConfidenceScore { get; set; }
        
        public PredictionClass? PredictedClass { get; set; }
        
        [MaxLength(2000)]
        public string? VoiceFeaturesJson { get; set; }
        
        [MaxLength(2000)]
        public string? GaitFeaturesJson { get; set; }
        
        [MaxLength(2000)]
        public string? WaveformDataJson { get; set; }
        
        [MaxLength(2000)]
        public string? SkeletonDataJson { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsSimulation { get; set; } = true;
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
    
    public enum AnalysisType
    {
        VoiceOnly = 0,
        GaitOnly = 1,
        MultiModal = 2
    }
    
    public enum PredictionClass
    {
        Healthy = 0,
        ParkinsonPositive = 1,
        Uncertain = 2
    }
}

